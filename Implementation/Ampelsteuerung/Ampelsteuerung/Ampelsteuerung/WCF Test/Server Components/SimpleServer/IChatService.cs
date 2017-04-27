using System.ServiceModel;

namespace SimpleServer
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatCallback))]

    public interface IChatService
    {
        [OperationContract]
        void getAmpelInformation(string ampelid, string ausfall);

    }
}
