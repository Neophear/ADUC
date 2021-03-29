using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Text;
using System.Security.AccessControl;
using System.Web.Security;
using Stiig;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Linq;

/// <summary>
/// Summary description for ADUser
/// </summary>
public class ADUser
{
    public string AccountName { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string FullName { get { return String.Format("{0} {1}", Firstname, Lastname); } }
    public string OU { get; set; }
    public List<string> Groups { get; set; }
    public DateTime? LastLogon
    {
        get { return up.LastLogon; }
    }
    public bool Locked { get; set; }
    public bool Enabled { get; set; }
    public DateTime? DateExpires { get; set; }

    public bool AccountExpired
    {
        get
        {
            return !DateExpires.HasValue && DateExpires.Value < DateTime.Today;
        }
    }

    private PrincipalContext ouContex;
    private UserPrincipal up;

    private ADUser()
    {

    }

    public ADUser(string accountname, string firstname, string lastname, string password, string ou, DateTime? expires = null)
    {
        ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", ou);
        up = new UserPrincipal(ouContex);

        this.AccountName = accountname.Trim();
        this.Firstname = firstname.Trim();
        this.Lastname = lastname.Trim();
        this.Password = password;
        this.OU = ou;
        this.Locked = false;
        this.DateExpires = expires;
    }

    /// <summary>
    /// Saves the user to AD
    /// </summary>
    /// <returns>Returns an Error object with information about eventual errors</returns>
    public CustomError Save(bool mustChangePassword, bool BULK)
    {
        CustomError errors = new CustomError();

        if (DoesUserExist(AccountName))
            errors.Add(CustomError.ErrorType.UserExists);
        else
        {
            try
            {
                up.SamAccountName = AccountName;
                up.UserPrincipalName = AccountName + "@TRR-INET.local";
                up.Surname = Lastname;
                up.GivenName = Firstname;
                up.DisplayName = AccountName;
                up.SetPassword(Password);
                up.AccountExpirationDate = DateExpires;
                up.Enabled = true;

                //up.HomeDrive = "P:";
                //up.HomeDirectory = (@"\\TRR-I-SRV2\Pdrev$\" + AccountName);

                if (mustChangePassword)
                    up.ExpirePasswordNow();

                up.Save();

                //CreatePDriveFolder();
                //DirectoryEntry entry = (DirectoryEntry)up.GetUnderlyingObject();
                //entry.InvokeSet("HomeDirectory", new object[] { (@"\\TRR-I-SRV2\Pdrev$\" + AccountName) });
                //entry.InvokeSet("HomeDrive", new object[] { "P:" });
                //entry.InvokeSet("ProfilePath", new object[] { (@"\\TRR-SRV1\UserProfiles$\" + AccountName) });
                //entry.CommitChanges();

                if (!BULK)
                    Logging.WriteLog(Logging.Action.Create, Logging.ObjectType.ADUser, AccountName, (!mustChangePassword ? Password : String.Empty), mustChangePassword, String.Format("{0}{1}", FullName, (DateExpires.HasValue ? (" - " + DateExpires.Value.ToShortDateString()) : String.Empty)));
            }
            catch (PrincipalExistsException)
            {
                errors.Add(CustomError.ErrorType.UserExists);
            }
            catch (PrincipalOperationException)
            {
                errors.Add(CustomError.ErrorType.OUDoesNotExist);
            }
            catch (InvalidOperationException)
            {
                errors.Add(CustomError.ErrorType.UnknownError);
            }
        }

        return errors;
    }

    public static CustomError Delete(string username)
    {
        CustomError errors = new CustomError();

        if (Roles.IsUserInRole("Admin"))
        {
            try
            {
                ADUser user = Find(username);

                if (user != null)
                {
                    try
                    {
                        user.up.Delete();
                        //if (Directory.Exists(@"\\TRR-I-SRV2\Pdrev$\" + username))
                        //    Directory.Delete(@"\\TRR-I-SRV2\Pdrev$\" + username);

                        //if (Directory.Exists(@"\\TRR-I-SRV2\UserProfiles$\" + username))
                        //    Directory.Delete(@"\\TRR-I-SRV2\UserProfiles$\" + username);
                    }
                    finally
                    {
                        Logging.WriteLog(Logging.Action.Delete, Logging.ObjectType.ADUser, username);
                    }

                }
                else
                    errors.Add(CustomError.ErrorType.UserDoesNotExist);
            }
            catch (ADUser.NoAccessToADUserException)
            {
                errors.Add(CustomError.ErrorType.NoAccess);
            }
        }
        else
            errors.Add(CustomError.ErrorType.NoAccess);

        return errors;
    }

    public static CustomError Move(string username, string newOU, string newOUName)
    {
        CustomError errors = new CustomError();

        try
        {
            ADUser user = Find(username);

            if (user != null)
            {
                try
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + user.up.DistinguishedName);
                    string newDN = "LDAP://" + newOU;
                    DirectoryEntry newEntry = new DirectoryEntry(newDN);

                    entry.MoveTo(newEntry);
                    Logging.WriteLog(Logging.Action.Edit, Logging.ObjectType.ADUser, user.AccountName, "", null, "Change OU to " + newOUName);
                }
                catch (DirectoryServicesCOMException)
                {
                    errors.Add(CustomError.ErrorType.OUDoesNotExist);
                }
            }
            else
                errors.Add(CustomError.ErrorType.UserDoesNotExist);
        }
        catch (ADUser.NoAccessToADUserException)
        {
            errors.Add(CustomError.ErrorType.NoAccess);
        }

        return errors;
    }
    public static bool DoesUserExist(string accountname)
    {
        using (var domainContext = new PrincipalContext(ContextType.Domain, "TRR-INET.local"))
            using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, accountname))
                return foundUser != null;
    }

    //public void CreatePDriveFolder()
    //{
    //    try
    //    {
    //        string folderName = @"\\TRR-I-SRV2\Pdrev$\" + AccountName;
    //        Directory.CreateDirectory(folderName);

    //        FolderACL(folderName);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //        //MessageBox.Show("Error: " + ex.ToString());
    //    }
    //}

    private void FolderACL(String folderPath)
    {
        FileSystemRights Rights;

        //What rights are we setting?

        Rights = FileSystemRights.FullControl;
        bool modified;
        InheritanceFlags none = new InheritanceFlags();
        none = InheritanceFlags.None;

        //set on dir itself
        FileSystemAccessRule accessRule = new FileSystemAccessRule(up.Sid, Rights, none, PropagationFlags.NoPropagateInherit, AccessControlType.Allow);
        DirectoryInfo dInfo = new DirectoryInfo(folderPath);
        DirectorySecurity dSecurity = dInfo.GetAccessControl();
        dSecurity.ModifyAccessRule(AccessControlModification.Set, accessRule, out modified);

        //Always allow objects to inherit on a directory 
        InheritanceFlags iFlags = new InheritanceFlags();
        iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

        //Add Access rule for the inheritance
        FileSystemAccessRule accessRule2 = new FileSystemAccessRule(up.Sid, Rights, iFlags, PropagationFlags.InheritOnly, AccessControlType.Allow);
        dSecurity.ModifyAccessRule(AccessControlModification.Add, accessRule2, out modified);

        dInfo.SetAccessControl(dSecurity);
    }

    public static CustomError ChangePassword(string accountname, string password, bool changePasswordAtLogon, bool noLogging = false)
    {
        return ChangePassword(accountname, password, changePasswordAtLogon, !changePasswordAtLogon, noLogging);
    }

    public static CustomError ChangePassword(string accountname, string password, bool changePasswordAtLogon, bool pwdGenerated, bool noLogging = false)
    {
        CustomError errors = new CustomError();

        if (!DoesUserExist(accountname))
            errors.Add(CustomError.ErrorType.UserDoesNotExist);
        else
        {
            errors = Utilities.CheckPassword(password, accountname);

            if (!errors.HasErrors)
            {
                try
                {
                    PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", Utilities.GetSearchOU());

                    UserPrincipal up = UserPrincipal.FindByIdentity(ouContex, accountname);

                    if (up == null)
                        errors.Add(CustomError.ErrorType.NoAccess);
                    else
                    {
                        up.SetPassword(password);

                        if (changePasswordAtLogon)
                        {
                            up.PasswordNeverExpires = false;
                            up.ExpirePasswordNow();
                            up.Enabled = true;
                            up.Save();
                        }

                        if (!noLogging)
                            Logging.WriteLog(Logging.Action.ChangePassword, Logging.ObjectType.ADUser, accountname, (pwdGenerated ? password : String.Empty), changePasswordAtLogon);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        return errors;
    }

    public static CustomError Unlock(string accountname, bool logging = true)
    {
        CustomError errors = new CustomError();

        if (!DoesUserExist(accountname))
            errors.Add(CustomError.ErrorType.UserDoesNotExist);
        else
        {
            try
            {
                PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", Utilities.GetSearchOU());

                UserPrincipal up = UserPrincipal.FindByIdentity(ouContex, accountname);

                if (up == null)
                    errors.Add(CustomError.ErrorType.NoAccess);
                else
                {
                    up.UnlockAccount();
                    if (logging)
                        Logging.WriteLog(Logging.Action.Unlock, Logging.ObjectType.ADUser, accountname);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        return errors;
    }

    public static CustomError SetExpires(string accountname, DateTime? expiresDate, bool logging = true)
    {
        CustomError errors = new CustomError();

        if (!DoesUserExist(accountname))
            errors.Add(CustomError.ErrorType.UserDoesNotExist);
        else
        {
            try
            {
                PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", Utilities.GetSearchOU());

                UserPrincipal up = UserPrincipal.FindByIdentity(ouContex, accountname);

                if (up == null)
                    errors.Add(CustomError.ErrorType.NoAccess);
                else
                {
                    if (up.AccountExpirationDate != expiresDate)
                    {
                        up.AccountExpirationDate = expiresDate;
                        up.Save();

                        if (logging)
                            Logging.WriteLog(Logging.Action.Expires, Logging.ObjectType.ADUser, accountname, "", null, (expiresDate.HasValue ? expiresDate.Value.ToShortDateString() : "Udløb fjernet"));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        return errors;
    }

    public static ADUser Find(string accountname)
    {
        ADUser user = new ADUser();

        if (!DoesUserExist(accountname))
            user = null;
        else
        {
            try
            {
                PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", Utilities.GetSearchOU());

                UserPrincipal up = UserPrincipal.FindByIdentity(ouContex, accountname);

                if (up != null)
                {
                    user.AccountName = up.SamAccountName;
                    user.Firstname = up.GivenName;
                    user.Lastname = up.Surname;
                    user.Locked = up.IsAccountLockedOut();
                    user.OU = up.DistinguishedName.Substring(up.DistinguishedName.IndexOf(',') + 1);
                    user.up = up;
                    user.DateExpires = up.AccountExpirationDate;
                    user.Enabled = up.Enabled == null ? true : up.Enabled.Value;

                    user.Groups = new List<string>();
                    PrincipalSearchResult<Principal> usersGroups = up.GetGroups();
                    IEnumerable<string> groupNames = usersGroups.Select(x => x.SamAccountName);
                    foreach (var name in groupNames)
                    {
                        if (name.StartsWith("T-") || name.StartsWith("IT"))
                        {
                            user.Groups.Add(name.ToString());
                        }
                    }
                }
                else
                    throw new NoAccessToADUserException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        return user;
    }

    public static int GetUserCount(string ou)
    {
        try
        {
            PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", ou);
            UserPrincipal up = new UserPrincipal(ouContex);
            PrincipalSearcher ps = new PrincipalSearcher();
            ps.QueryFilter = up;
            ((DirectorySearcher)ps.GetUnderlyingSearcher()).PageSize = 10000;
            int count = 0;

            foreach (Principal p in ps.FindAll())
                count++;

            return count;
        }
        catch (Exception)
        {
            return -1;
        }
    }

    public static List<Principal> GetGroups()
    {
        List<Principal> groups = new List<Principal>();
        // create your domain context
        PrincipalContext ouContex = new PrincipalContext(ContextType.Domain);

        // define a "query-by-example" principal - here, we search for a GroupPrincipal 
        GroupPrincipal qbeGroup = new GroupPrincipal(ouContex);

        // create your principal searcher passing in the QBE principal    
        PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

        // find all matches
        foreach (var found in srch.FindAll())
        {
            // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....          
            if (found.Name.StartsWith("T-") || found.Name.StartsWith("IT"))
            {
                groups.Add(found);
            }
        }

        return groups.OrderBy(i => i.Name).ToList();
        //return groups.OrderBy(i => i.Name.Length).ToList();
    }

    public static CustomError AddUserToGroup(string accountname, List<string> groups, List<string> removeFromGroups, bool logging = true)
    {

        CustomError errors = new CustomError();
        string lognote;
        if (!DoesUserExist(accountname))
            errors.Add(CustomError.ErrorType.UserDoesNotExist);
        else
        {
            try
            {
                ADUser user = Find(accountname);

                PrincipalContext ouContex = new PrincipalContext(ContextType.Domain);
                UserPrincipal up = UserPrincipal.FindByIdentity(ouContex, user.AccountName);

                foreach (var item in groups)
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(ouContex, item);

                    if (!user.Groups.Contains(item))
                    {
                        group.Members.Add(up);
                        group.Save();
                        lognote = "Tilføjet til: " + item;
                        if (logging)
                            Logging.WriteLog(Logging.Action.Edit, Logging.ObjectType.ADUser, user.AccountName, "", null, lognote);
                    }
                }
                
                foreach (var item in removeFromGroups)
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(ouContex, item);

                    if (user.Groups.Contains(item))
                    {
                        group.Members.Remove(up);
                        group.Save();
                        lognote = "Fjernet fra: " + item;
                        if (logging)
                            Logging.WriteLog(Logging.Action.Edit, Logging.ObjectType.ADUser, user.AccountName, "", null, lognote);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        return errors;
    }

    public class NoAccessToADUserException : Exception
    {
        public NoAccessToADUserException()
            : base("No access to User Object")
        {

        }
    }

    public static CustomError Mail(string accountname, string Password, bool logging = true)
    {
        CustomError errors = new CustomError();

        if (!DoesUserExist(accountname))
            errors.Add(CustomError.ErrorType.UserDoesNotExist);
        else
        {
            try
            {
                ADUser user = Find(accountname);
                Utilities.SendMail(user.FullName, accountname, Password);
                if (logging)
                    Logging.WriteLog(Logging.Action.Mail, Logging.ObjectType.ADUser, accountname, "", null, "Mail sendt");
            }
            catch (Exception)
            {
                throw;
            }
        }

        return errors;
    }
}

public class ADUCUser
{
    private MembershipUser _user;
    private ProfileCommon _profile;
    public string MANR { get { return _user.UserName; } }
    public string Firstname { get { return _profile.Firstname; } }
    public string Lastname { get { return _profile.Lastname; } }
    public string Fullname { get { return String.Format("{0} {1}", _profile.Firstname, _profile.Lastname); } }
    public string DisplayName { get { return String.Format("{0} - {1} {2}", MANR, Firstname, Lastname); } }
    public bool IsLockedOut { get { return _user.IsLockedOut; } }
    public bool IsApproved { get { return _user.IsApproved; } }
    public bool IsAdmin { get { return Roles.IsUserInRole(MANR, "Admin"); } }
    public DateTime? LastLoginDate
    {
        get
        {
            bool hasLoggedIn = _user.LastLoginDate == _user.CreationDate;
            DateTime? lastLoginDate = _user.LastLoginDate;
            return hasLoggedIn ? null : lastLoginDate;
        }
    }
    public DateTime CreatedDate { get { return _user.CreationDate; } }

    public ADUCUser(MembershipUser user, ProfileCommon profile)
    {
        _user = user;
        _profile = profile;
    }

    public static List<ADUCUser> GetList(MembershipUserCollection users, ProfileCommon profile)
    {
        List<ADUCUser> result = new List<ADUCUser>();

        foreach (MembershipUser user in users)
            result.Add(new ADUCUser(user, profile.GetProfile(user.UserName)));

        return result;
    }
}

public class Logging
{
    /// <summary>
    /// Writes a log entry
    /// </summary>
    /// <param name="a">Action</param>
    /// <param name="o">ObjectType</param>
    /// <param name="on">The affected objects name</param>
    /// <param name="p">An eventual password</param>
    /// <param name="m">If user has to change password at logon</param>
    /// <param name="n">A note to pass</param>
    public static void WriteLog(Action a, ObjectType o, string on, string p = "", bool? m = null, string n = "")
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@Action", a, DbType.Int16);
        dal.AddParameter("@ObjectType", o, DbType.Int16);
        dal.AddParameter("@Username", Membership.GetUser().UserName, DbType.String);
        dal.AddParameter("@ObjectName", on, DbType.String);
        dal.AddParameter("@Password", p, DbType.String);
        dal.AddParameter("@MustChangePassword", m ?? System.Data.SqlTypes.SqlBoolean.Null, DbType.Boolean);
        dal.AddParameter("@Note", n, DbType.String);
        dal.ExecuteNonQuery("EXEC dbo.WriteLog @Action, @ObjectType, @Username, @ObjectName, @Password, @MustChangePassword, @Note");
        dal.ClearParameters();
    }

    public static int CreateNewBULK(bool ChangePasswordIfExist)
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@Username", Membership.GetUser().UserName, DbType.String);
        dal.AddParameter("@ChangePasswordIfExist", ChangePasswordIfExist, DbType.Boolean);
        object lastID = dal.ExecuteScalar("DECLARE @BULKID INT;EXEC @BULKID = dbo.BULKCreate @Username, @ChangePasswordIfExist;SELECT @BULKID");
        dal.ClearParameters();

        return (int)lastID;
    }

    public static void WriteBULKDetail(int BULKID, int rownumber, string adusername, string firstname, string lastname, string password, string ouname)
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@BULKID", BULKID, DbType.Int32);
        dal.AddParameter("@RowNumber", rownumber, DbType.Int32);
        dal.AddParameter("@ADUsername", adusername, DbType.String);
        dal.AddParameter("@Firstname", firstname, DbType.String);
        dal.AddParameter("@Lastname", lastname, DbType.String);
        dal.AddParameter("@Password", password, DbType.String);
        dal.AddParameter("@OUName", ouname, DbType.String);
        dal.ExecuteNonQuery("EXEC dbo.WriteBULKDetail @BULKID, @RowNumber, @ADUsername, @Firstname, @Lastname, @OUName, @Password");
        dal.ClearParameters();
    }

    public static void WriteChangeLog(string text, int? id = null)
    {
        DataAccessLayer dal = new DataAccessLayer();
        dal.AddParameter("@Text", text, DbType.String);
        dal.AddParameter("@ID", id ?? System.Data.SqlTypes.SqlInt32.Null, DbType.Int32);
        dal.ExecuteNonQuery("EXEC dbo.WriteChangeLog @Text, @ID");
        dal.ClearParameters();
    }

    /// <summary>
    /// Updates BULKDetail as NOT successful
    /// </summary>
    /// <param name="detailID"></param>
    /// <param name="message"></param>
    public static void UpdateBULKDetail(int detailID, string message)
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@ID", detailID, DbType.Int32);
        dal.AddParameter("@Message", message, DbType.String);
        dal.ExecuteNonQuery("UPDATE [BULKDetails] SET [Successful] = 0, [Message] = @Message WHERE [ID] = @ID");
        dal.ClearParameters();
    }

    public static void UpdateBULKDetail(int detailID, bool successful, string password, bool mustChangePassword, string message = "")
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@ID", detailID, DbType.Int32);
        dal.AddParameter("@Successful", successful, DbType.Boolean);
        dal.AddParameter("@Password", password, DbType.String);
        dal.AddParameter("@MustChangePassword", mustChangePassword, DbType.Boolean);
        dal.AddParameter("@Message", message, DbType.String);
        dal.ExecuteNonQuery("UPDATE [BULKDetails] SET [Successful] = @Successful, [Password] = @Password, [MustChangePassword] = @MustChangePassword, [Message] = @Message WHERE [ID] = @ID");
        dal.ClearParameters();
    }

    public static void UpdateBULK(int bulkID, string message, bool successful = true)
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@ID", bulkID, DbType.Int32);
        dal.AddParameter("@UpdatedBy", Membership.GetUser().UserName, DbType.String);
        dal.AddParameter("@Message", message, DbType.String);
        dal.AddParameter("@Approved", successful, DbType.Boolean);
        dal.ExecuteNonQuery("EXEC dbo.BULKUpdate @ID, @UpdatedBy, @Message, @Approved");
        dal.ClearParameters();
    }

    public enum Action
    {
        Create = 1,
        Delete = 2,
        Unlock = 3,
        ChangePassword = 4,
        Edit = 5,
        Expires = 6,
        Mail = 7,
    }

    public enum ObjectType
    {
        ADUser = 1,
        ADUCUser = 2,
        OU = 3,
        BULK = 4
    }
}

public class CustomError
{
    public List<ErrorType> ErrorList { get; set; }
    public int Count { get { return ErrorList.Count; } }
    public bool HasErrors { get { return ErrorList.Count > 0; } }

    public CustomError()
    {
        ErrorList = new List<ErrorType>();
    }

    public CustomError(ErrorType type)
        : this()
    {
        ErrorList.Add(type);
    }

    public void Add(ErrorType type)
    {
        ErrorList.Add(type);
    }

    public void Add(CustomError errors)
    {
        foreach (ErrorType type in errors.ErrorList)
        {
            if (!ErrorList.Contains(type))
                Add(type);
        }
    }

    public void Clear()
    {
        ErrorList.Clear();
    }

    public enum ErrorType
    {
        UserExists,
        UserDoesNotExist,
        OUDoesNotExist,
        PasswordLessThan8,
        PasswordIncludesAccount,
        PasswordNotComplex,
        NoUsername,
        NoFirstname,
        NoLastname,
        NoAccess,
        UnknownError
    }

    public static CustomError operator +(CustomError e1, CustomError e2)
    {
        CustomError result = new CustomError();
        result.Add(e1);
        result.Add(e2);
        return result;
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        if (HasErrors)
        {
            result.Append("Følgende fejl fundet: ");

            foreach (ErrorType type in ErrorList)
            {
                switch (type)
                {
                    case ErrorType.UserExists:
                        result.Append("MANR findes i forvejen, ");
                        break;
                    case ErrorType.UserDoesNotExist:
                        result.Append("MANR findes ikke, ");
                        break;
                    case ErrorType.OUDoesNotExist:
                        result.Append("OU findes ikke, ");
                        break;
                    case ErrorType.PasswordLessThan8:
                        result.Append("password skal være længere end 8 tegn, ");
                        break;
                    case ErrorType.PasswordIncludesAccount:
                        result.Append("password indeholder MANR, ");
                        break;
                    case ErrorType.PasswordNotComplex:
                        result.Append("password er ikke komplekst nok, ");
                        break;
                    case ErrorType.NoUsername:
                        result.Append("intet MANR indtastet, ");
                        break;
                    case ErrorType.NoFirstname:
                        result.Append("intet fornavn indtastet, ");
                        break;
                    case ErrorType.NoLastname:
                        result.Append("intet efternavn indtastet, ");
                        break;
                    case ErrorType.NoAccess:
                        result.Append("ingen adgang, ");
                        break;
                    case ErrorType.UnknownError:
                        result.Append("ukendt fejl (kontakt os), ");
                        break;
                    default:
                        break;
                }
            }

            result.Remove(result.Length - 2, 2);
        }
        else
            result.Append("Ingen fejl fundet");

        return result.ToString();
    }
}