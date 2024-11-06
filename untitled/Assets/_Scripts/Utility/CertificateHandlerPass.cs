using UnityEngine.Networking;

public class CertificateHandlerPass : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
