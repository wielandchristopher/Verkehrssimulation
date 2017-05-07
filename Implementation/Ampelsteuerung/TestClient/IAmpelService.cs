using System.ServiceModel;

namespace Ampelsteuerung
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAmpelCallback))]

    public interface IAmpelService
    {
        //Erstellt die mitgegebene Menge an Ampeln
        [OperationContract]
        void setAmpelAnzahl(int anzahl);

        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt Status der Ampel zurück 
        [OperationContract]
        void getAmpelStatus(int ampelid);

        //Gibt integer und boolean zurück: 
        //integer ist die AmpelID
        //true - Ampel Funktioniert, false - Ampel ausgeschaltet
        [OperationContract]
        void getAmpelAusfall(int ampelid);

        //Schaltet die mitgegebene Ampel wieder aus
        [OperationContract]
        void setAmpelAusfall(int ampelid);

        //Schaltet die mitgegebene Ampel wieder ein. 
        [OperationContract]
        void setAmpelOn(int ampelid);

        //Setzt den AmpelStatus der Ampel (Rot, gelb, Grün, oder Ausfall) 
        // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
        [OperationContract]
        void setAmpelStatus(int ampelid, int neuerStatus);

        //Setzt die Rotphase einer einzelnen Ampel. Zeitangabe in Sekunden
        [OperationContract]
        void setRotPhase(int ampelid, int zeit);

        //Setzt die Gelbphase einer einzelnen Ampel. Zeitangabe in Sekunden
        [OperationContract]
        void setGelbPhase(int ampelid, int zeit);

        //Setzt die Grünphase einer einzelnen Ampel. Zeitangabe in Sekunden
        [OperationContract]
        void setGruenPhase(int ampelid, int zeit);

        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt die Sekunden der Rotphase zurück
        [OperationContract]
        void getRotPhase(int ampelid);

        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt die Sekunden der Gelbphase zurück
        [OperationContract]
        void getGelbPhase(int ampelid);

        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt die Sekunden der Grünphase zurück
        [OperationContract]
        void getGruenPhase(int ampelid);
    }
}
