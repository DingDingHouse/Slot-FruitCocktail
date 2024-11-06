#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Tls.Crypto.Impl.BC
{
    public class BcX448Domain
        : TlsECDomain
    {
        protected readonly BcTlsCrypto m_crypto;

        public BcX448Domain(BcTlsCrypto crypto)
        {
            this.m_crypto = crypto;
        }

        public virtual TlsAgreement CreateECDH()
        {
            return new BcX448(m_crypto);
        }
    }
}
#pragma warning restore
#endif
