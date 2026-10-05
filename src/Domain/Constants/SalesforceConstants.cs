namespace Domain.Constants;

public static class SalesforceConstants
{
    public const string TokenEndpointPath = "/services/oauth2/token";
    public const string DataPath = "/services/data/";
    public const string SObjectsPath = "/sobjects/";
    public const string GrantTypeKey = "grant_type";
    public const string ClientIdKey = "client_id";
    public const string ClientSecretKey = "client_secret";
    public const string UsernameKey = "username";
    public const string PasswordKey = "password";
    public const string PasswordGrant = "password";
    public const string ClientCredentialsGrant = "client_credentials";
    public const string AccessTokenProperty = "access_token";
    public const string InstanceUrlProperty = "instance_url";
    public const string IdProperty = "id";
    public const string BearerScheme = "Bearer";
    public const string JsonMediaType = "application/json";
    public const string AccountObject = "Account";
    public const string ContactObject = "Contact";
    public const string FieldName = "Name";
    public const string FieldDescription = "Description";
    public const string FieldPhone = "Phone";
    public const string FieldFirstName = "FirstName";
    public const string FieldLastName = "LastName";
    public const string FieldEmail = "Email";
    public const string FieldMailingCity = "MailingCity";
    public const string FieldAccountId = "AccountId";
    public const string UnknownLastName = "Unknown";
    public const string SourceName = "CurricuVita";
    public const string LocationPrefix = " Location: ";
    public const string NotesPrefix = " Notes: ";
}
