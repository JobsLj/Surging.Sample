using System.Net;

namespace Surging.Core.CPlatform.Address
{
    /// <summary>
    /// ip��ַģ�͡�
    /// </summary>
    public sealed class IpAddressModel : AddressModel
    {
        #region Constructor

        /// <summary>
        /// ��ʼ��һ���µ�ip��ַģ��ʵ����
        /// </summary>
        public IpAddressModel()
        {
        }

        /// <summary>
        /// ��ʼ��һ���µ�ip��ַģ��ʵ����
        /// </summary>
        /// <param name="ip">ipaddress��</param>
        /// <param name="port">�˿ڡ�</param>
        public IpAddressModel(string address, int port)
        {
            _ip = AddressHelper.GetIpFromAddress(address);
            _port = port;
        }

        #endregion Constructor

        #region Property

        private string _ip;
        private int _port;

        /// <summary>
        /// ip��ַ��
        /// </summary>
        public string Ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = AddressHelper.GetIpFromAddress(value);
            }
        }

        /// <summary>
        /// �˿ڡ�
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        #endregion Property

        #region Overrides of AddressModel

        /// <summary>
        /// �����ս�㡣
        /// </summary>
        /// <returns></returns>
        public override EndPoint CreateEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(_ip), _port);
        }

        public override string ToString()
        {
            return string.Concat(new string[] { _ip, ":", _port.ToString() });
        }

        #endregion Overrides of AddressModel
    }
}