﻿namespace Adventour.Api.Constants.Database
{
    public static class StoredProcedures
    {
        public const string CheckUserExistsByEmail = "CheckUserExistsByEmail";
        public const string CheckUserExistsById = "CheckUserExistsById";
        public const string CreateUser = "CreateUser";
        public const string UpdateUserPublicData = "UpdateUserPublicData";
        public const string GetCountryByCode = "GetCountryByCode";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string GetPersonByOAuthId = "GetPersonByOAuthId";
        public const string GetDaysByItineraryId = "GetDaysByItineraryId";

        public static class Parameters
        {
            public const string UserId = "@userId";
            public const string Email = "@email";
            public const string Name = "@name";
            public const string Password = "@password";
            public const string Username = "@username";
            public const string PhotoUrl = "@photoUrl";
            public const string Code = "@code";
            public const string OAuthId = "@oAuthId";
            public const string ItineraryId = "@itineraryId";

        }
    }
}
