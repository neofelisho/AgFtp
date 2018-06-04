using System;
using System.Globalization;
using System.Xml.Serialization;

namespace JH.AgFtp
{
    [Serializable]
    [XmlRoot(ElementName = "row")]
    public class AgHunter : IAgDataObject
    {
        [XmlAttribute("dataType")]
        public string DataType { get; set; }

        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute("tradeNo")]
        public string TradeNumber { get; set; }

        [XmlAttribute("platformType")]
        public string PlatformType { get; set; }

        [XmlAttribute("sceneId")]
        public string SceneId { get; set; }

        [XmlAttribute("playerName")]
        public string PlayerName { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("SceneStartTime")]
        public string SceneStartTimeString
        {
            get => SceneStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            set => SceneStartTime = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime SceneStartTime { get; set; }

        [XmlAttribute("SceneEndTime")]
        public string SceneEndTimeString
        {
            get => SceneEndTime.ToString("yyyy-MM-dd HH:mm:ss");
            set => SceneEndTime = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime SceneEndTime { get; set; }

        [XmlAttribute("Roomid")]
        public string RoomId { get; set; }

        [XmlAttribute("Roombet")]
        public string RoomBet { get; set; }

        [XmlAttribute("Cost")]
        public string Cost { get; set; }

        [XmlAttribute("Earn")]
        public string Earn { get; set; }

        [XmlAttribute("Jackpotcomm")]
        public string JackpotComm { get; set; }


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
        public string ExchangeRate { get; set; }

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

        [XmlAttribute("gameCode")]
        public string GameCode { get; set; }

        [XmlAttribute("deviceType")]
        public string DeviceType { get; set; }
    }
}