namespace Finance.PciDss.Bridge.IceMarket.Server.Services.Integrations.Contracts.Enums
{
    public static class IceMarketResultCode
    {
        public static string Success = "0000";
        public static string ExpiredCard = "0101";
        public static string FraudSuspitionOrTransitoryException = "0102";
        public static string NotAllowed = "0104";
        public static string PinAttempsExceeded = "0106";
        public static string ContactCardIssuer = "0107";
        public static string InvalidIdentification = "0109";
        public static string InvalidAmount = "0110";
        public static string CandCannotBeUsed = "0114";
        public static string AvailableInsufficient = "0116";
        public static string UnregisteredCard = "0118";
        public static string NoSpecificReasonRefusal = "0119";
        public static string CardNotEffective = "0125";
        public static string IncorrectCvv2 = "0129";
        public static string SuspectedFaurd = "0167";
        public static string CardOutsideSirvice = "0180";
        public static string CardWithCreditOrDebitRestriction = "0181";
        public static string CardWithCreditOrDebitRestriction2 = "0182";
        public static string HolderAuthentificationFailed = "0184";
        public static string DenialWithodSpecifyingReason = "0190";
        public static string IncorrectExpirationDate = "0191";
        public static string CardExpired = "0201";
        public static string CardBlocked = "0202";
        public static string TransactionNotPermitted = "0204";
        public static string CantactCardIssure = "0207";
        public static string LostOrStolenCard = "0208";
        public static string LostOrStolenCard2 = "0209";
        public static string Cvv2Error = "0280";
        public static string RefusalWWithNoSpecificReason = "0290";
        public static string Timeout = "480";
        public static string Timeout2 = "501";
        public static string Timeout3 = "502";
        public static string Timeout4 = "503";
        public static string CommerceNotRegisteredInFuc = "0904";
        public static string SystemError = "0909";
        public static string IssuerNotAvailable = "0912";
        public static string RepeatedOrder = "0913";
        public static string IncorrectSession = "0944";
        public static string ReturnOperationNotAllowed = "0964";
        public static string IncorectNumberOfCardPositions = "0964";

        public static string NoValidPaymentMethodForCard = "0978";
        public static string RejectionOfInternationalServers = "9094";
        public static string NoSecureKey = "9104";
        public static string DoNotAllowed = "9218";
        public static string CardNotCompyWithCheckDigit = "9253";
        public static string MerchantCannotCarrayPreAuthorization = "9256";
    }

}
