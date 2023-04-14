using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleTest.Test
{
    public class GenX509Certification
    {
        public GenX509Certification()
        {
            File.WriteAllBytes("d:/test.pfx", Generate("CN=Realwith", "realwith@0622"));
        }
        public byte[] Generate(string name, string password)
        {
            using RSA parent = RSA.Create(4096);
            using RSA rsa = RSA.Create(2048);

            CertificateRequest parentReq = new CertificateRequest(name,
                parent,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            parentReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
            parentReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(parentReq.PublicKey, false));

            using var parentCert = parentReq.CreateSelfSigned(
                DateTimeOffset.UtcNow.AddDays(-45),
                DateTimeOffset.UtcNow.AddDays(365));

            var req = new CertificateRequest(name,
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            req.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            req.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, false));
            req.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.8") }, true));
            req.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(parentReq.PublicKey, false));

            using var cert = req.Create(parentCert,
                DateTimeOffset.UtcNow.AddDays(-1),
                DateTimeOffset.UtcNow.AddDays(90),
                new byte[] { 1, 2, 3, 4 });
            return cert.Export(X509ContentType.Pfx, password);
        }
    }
}
