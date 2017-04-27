using System.ServiceModel;

namespace Interfaces
{
    [ServiceContract(
         SessionMode = SessionMode.Required, 
         CallbackContract = typeof(IChatCallback))]

    public interface IChatService
    {
        [OperationContract]
        void SendMessage(string msg);

        [OperationContract]
        bool Subscribe();

        [OperationContract]
        bool Unsubscribe();
    }
}
