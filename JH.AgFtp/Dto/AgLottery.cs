﻿using System;
using System.Globalization;
using System.Xml.Serialization;

namespace JH.AgFtp
{
    [Serializable]
    [XmlRoot(ElementName = "row")]
    public class AgLottery : IAgDataObject
    {
        [XmlAttribute("dataType")]
        public string DataType { get; set; }

        [XmlAttribute("billNo")]
        public string BillNumber { get; set; }

        [XmlAttribute("playerName")]
        public string PlayerName { get; set; }

        [XmlAttribute("agentCode")]
        public string AgentCode { get; set; }

        [XmlAttribute("gameCode")]
        public string GameCode { get; set; }

        [XmlAttribute("netAmount")]
        public string NetAmountString
        {
            get => NetAmount.ToString(CultureInfo.InvariantCulture);
            set => NetAmount = decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal NetAmount { get; set; }

        [XmlAttribute("betTime")]
        public string BetTimeString
        {
            get => BetTime.ToString("yyyy-MM-dd HH:mm:ss");
            set => BetTime = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime BetTime { get; set; }

        [XmlAttribute("gameType")]
        public string GameType { get; set; }

        [XmlAttribute("betAmount")]
        public string BetAmountString
        {
            get => BetAmount.ToString(CultureInfo.InvariantCulture);
            set => BetAmount = decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal BetAmount { get; set; }

        [XmlAttribute("validBetAmount")]
        public string ValidBetAmountString
        {
            get => ValidBetAmount.ToString(CultureInfo.InvariantCulture);
            set => ValidBetAmount = decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal ValidBetAmount { get; set; }

        [XmlAttribute("flag")]
        public string Flag { get; set; }

        [XmlAttribute("playType")]
        public string PlayType { get; set; }

        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlAttribute("tableCode")]
        public string TableCode { get; set; }

        [XmlAttribute("loginIP")]
        public string LoginIp { get; set; }

        [XmlAttribute("recalcuTime")]
        public string ReCalculateTimeString
        {
            get => ReCalculateTime.ToString("yyyy-MM-dd HH:mm:ss");
            set => ReCalculateTime = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime ReCalculateTime { get; set; }

        [XmlAttribute("platformType")]
        public string PlatformType { get; set; }

        [XmlAttribute("remark")]
        public string Remark { get; set; }

        [XmlAttribute("round")]
        public string Round { get; set; }

        [XmlAttribute("result")]
        public string Result { get; set; }

        [XmlAttribute("beforeCredit")]
        public string BeforeCreditString
        {
            get => BeforeCredit.ToString(CultureInfo.InvariantCulture);
            set => BeforeCredit = string.IsNullOrEmpty(value) ? 0 : decimal.Parse(value);
        }

        [XmlIgnore]
        public decimal BeforeCredit { get; set; }

        [XmlAttribute("deviceType")]
        public string DeviceType { get; set; }
    }
}