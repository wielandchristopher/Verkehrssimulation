using System.ServiceModel;

namespace Ampelsteuerung
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAmpelCallback))]

    public interface IAmpelService
    {
        [OperationContract]
        void getAmpelInformation(int ampelid, string ausfall);

    }
}
