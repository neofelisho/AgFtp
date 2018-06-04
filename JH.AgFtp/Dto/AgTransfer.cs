using System;
using System.Globalization;
using System.Xml.Serialization;

namespace JH.AgFtp
{
    [Serializable]
    [XmlRoot(ElementName = "row")]
    public class AgTransfer : IAgDataObject
    {
        [XmlAttribute("dataType")]
        public string DataType { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute("agentCode")]
        public string AgentCode { get; set; }

        [XmlAttribute("transferId")]
        public string TransferId { get; set; }

        [XmlAttribute("tradeNo")]
        public string TradeNumber { get; set; }

        [XmlAttribute("platformType")]
        public string PlatformType { get; set; }

        [XmlAttribute("playerName")]
        public string PlayerName { get; set; }

        [XmlAttribute("transferType")]
        public string TransferType { get; set; }

        [XmlAttribute("transferAmount")]
        public string TransferAmountString
        {
            get => TransferAmount.ToString(CultureInfo.InvariantCulture);
            set => TransferAmount = decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal TransferAmount { get; set; }


        [XmlAttribute("previousAmount")]
        public string PreviousAmountString
        {
            get => PreviousAmount.ToString(CultureInfo.InvariantCulture);
            set => PreviousAmount = decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal PreviousAmount { get; set; }

        [XmlAttribute("currentAmount")]
        public string CurrentAmountString
        {
            get => CurrentAmount.ToString(CultureInfo.InvariantCulture);
            set => CurrentAmount = decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal CurrentAmount { get; set; }


        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlAttribute("exchangeRate")]
        public string ExchangeRateString
        {
            get => ExchangeRate.ToString(CultureInfo.InvariantCulture);
            set => ExchangeRate = double.Parse(value);
        }

        [XmlIgnore]
        public double ExchangeRate { get; set; }

        [XmlAttribute("IP")]
        public string Ip { get; set; }

        [XmlAttribute("flag")]
        public string Flag { get; set; }


        [XmlAttribute("creationTime")]
        public string CreateTimeString
        {
            get => CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            set => CreateTime = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime CreateTime { get; set; }

        [XmlAttribute("remark")]
        public string Remark { get; set; }

        [XmlAttribute("gameCode")]
        public string GameCode { get; set; }
    }
}