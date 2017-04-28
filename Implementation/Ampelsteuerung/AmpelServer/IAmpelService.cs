using System.ServiceModel;

namespace Ampelsteuerung
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAmpelCallback))]

    public interface IAmpelService
    {
        [OperationContract]
        void getAmpelStatus(int ampelid);

        [OperationContract]
        void getAmpelAusfall(int ampelid);

        [OperationContract]
        void setAmpelAusfall(int ampelid);

        [OperationContract]
        void setAmpelOn(int ampelid);

        [OperationContract]
        void setAmpelStatus(int ampelid, int neuerStatus);

        [OperationContract]
        void setRotPhase(int ampelid, int zeit);

        [OperationContract]
        void setGelbPhase(int ampelid, int zeit);

        [OperationContract]
        void setGruenPhase(int ampelid, int zeit);

        [OperationContract]
        void getRotPhase(int ampelid);

        [OperationContract]
        void getGelbPhase(int ampelid);

        [OperationContract]
        void getGruenPhase(int ampelid);
    }
}
