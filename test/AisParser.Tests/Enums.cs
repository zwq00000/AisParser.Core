namespace AisParser.Tests {

    public enum AisMessageType {
        PositionReportClassA = 1,
        PositionReportClassAAssignedSchedule = 2,
        PositionReportClassAResponseToInterrogation = 3,
        BaseStationReport = 4,
        StaticAndVoyageRelatedData = 5,
        BinaryAddressedMessage = 6,
        BinaryAcknowledge = 7,
        BinaryBroadcastMessage = 8,
        StandardSarAircraftPositionReport = 9,
        UtcAndDateInquiry = 10,
        UtcAndDateResponse = 11,
        AddressedSafetyRelatedMessage = 12,
        SafetyRelatedAcknowledgement = 13,
        SafetyRelatedBroadcastMessage = 14,
        Interrogation = 15,
        AssignmentModeCommand = 16,
        DgnssBinaryBroadcastMessage = 17,
        StandardClassBCsPositionReport = 18,
        ExtendedClassBCsPositionReport = 19,
        DataLinkManagement = 20,
        AidToNavigationReport = 21,
        ChannelManagement = 22,
        GroupAssignmentCommand = 23,
        StaticDataReport = 24,
        SingleSlotBinaryMessage = 25,
        MultipleSlotBinaryMessageWithCommunicationsState = 26,
        PositionReportForLongRangeApplications = 27
    }

    /// <summary>
    /// The position accuracy (PA) flag should be determined in accordance with Table 50 
    /// 1 = high (<=10 m) 
    /// 0 = low (>10 m)
    /// 0 = default
    /// </summary>
    public enum PositionAccuracy {
        /// <summary>
        /// 0 = default
        /// </summary>
        Default = 0,
        /// <summary>
        /// 0 = low (>10 m)
        /// </summary>
        Low = 0,
        /// <summary>
        /// 1 = high (<=10 m) 
        /// </summary>
        High = 1
    }

    public enum ManeuverIndicator {
        NotAvailable,
        NoSpecialManeuver,
        SpecialManeuver
    }

    public enum NavigationStatus {
        UnderWayUsingEngine,
        AtAnchor,
        NotUnderCommand,
        RestrictedManeuverability,
        ConstrainedByHerDraught,
        Moored,
        Aground,
        EngagedInFishing,
        UnderWaySailing,
        ReservedForFutureAmendmentOfNavigationalStatusForHsc,
        ReservedForFutureAmendmentOfNavigationalStatusForWig,
        ReservedForFutureUse1,
        ReservedForFutureUse2,
        ReservedForFutureUse3,
        AisSartIsActive,
        NotDefined
    }

    /// <summary>
    /// 电子定位装置 的类型 
    /// </summary>
    /// <remarks>
    /// 0 = 未规定（默认值） 
    //  1 = GPS 
    //  2 = GLONASS 
    //  3 = GPS/GLONASS组合 
    //  4 = Loran-C 
    //  5 = Chayka 
    //  6 = 综合导航系统 
    //  7 = 正在研究 
    //  8 = Galileo 
    //  9-14 = 未使用 
    //  15 = 内部GNSS
    /// </remarks>
    public enum PositionFixType {

        /// <summary>
        /// 未规定
        /// </summary>
        Undefined1 = 0,
        /// <summary>
        /// GPS
        /// </summary>
        Gps = 1,
        /// <summary>
        /// GLONASS
        /// </summary>
        Glonass = 2,
        /// <summary>
        /// GPS/GLONASS组合
        /// </summary>
        CombinedGpsAndGlonass = 3,
        /// <summary>
        /// Loran-C 
        /// </summary>
        LoranC = 4,
        /// <summary>
        /// Chayka 
        /// </summary>
        Chayka = 5,
        /// <summary>
        /// 综合导航系统
        /// </summary>
        IntegratedNavigationSystem = 6,
        /// <summary>
        /// 正在研究
        /// </summary>
        Surveyed = 7,
        /// <summary>
        /// 伽利略卫星定位
        /// </summary>
        Galileo = 8,
        /// <summary>
        /// 内部GNSS
        /// </summary>
        InternalGNSS = 15
    }

    public enum Raim {
        NotInUse,
        InUse
    }

    public enum ShipType {
        NotAvailable,
        WingInGround = 20,
        WingInGroundHazardousCategoryA = 21,
        WingInGroundHazardousCategoryB = 22,
        WingInGroundHazardousCategoryC = 23,
        WingInGroundHazardousCategoryD = 24,
        WingInGroundReserved1 = 25,
        WingInGroundReserved2 = 26,
        WingInGroundReserved3 = 27,
        WingInGroundReserved4 = 28,
        WingInGroundReserved5 = 29,
        Fishing = 30,
        Towing = 31,
        TowingLarge = 32,
        DredgingOrUnderwaterOps = 33,
        DivingOps = 34,
        MilitaryOps = 35,
        Sailing = 36,
        PleasureCraft = 37,
        Reserved1 = 38,
        Reserved2 = 39,
        HighSpeedCraft = 40,
        HighSpeedCraftHazardousCategoryA = 41,
        HighSpeedCraftHazardousCategoryB = 42,
        HighSpeedCraftHazardousCategoryC = 43,
        HighSpeedCraftHazardousCategoryD = 44,
        HighSpeedCraftReserved1 = 45,
        HighSpeedCraftReserved2 = 46,
        HighSpeedCraftReserved3 = 47,
        HighSpeedCraftReserved4 = 48,
        HighSpeedCraftNoAdditionalInformation = 49,
        PilotVessel = 50,
        SearchAndRescueVessel = 51,
        Tug = 52,
        PortTender = 53,
        AntiPollutionEquipment = 54,
        LawEnforcement = 55,
        SpareLocalVessel1 = 56,
        SpareLocalVessel2 = 57,
        MedicalTransport = 58,
        NonCombatantShip = 59,
        Passenger = 60,
        PassengerHazardousCategoryA = 61,
        PassengerHazardousCategoryB = 62,
        PassengerHazardousCategoryC = 63,
        PassengerHazardousCategoryD = 64,
        PassengerReserved1 = 65,
        PassengerReserved2 = 66,
        PassengerReserved3 = 67,
        PassengerReserved4 = 68,
        PassengerNoAdditionalInformation = 69,
        Cargo = 70,
        CargoHazardousCategoryA = 71,
        CargoHazardousCategoryB = 72,
        CargoHazardousCategoryC = 73,
        CargoHazardousCategoryD = 74,
        CargoReserved1 = 75,
        CargoReserved2 = 76,
        CargoReserved3 = 77,
        CargoReserved4 = 78,
        CargoNoAdditionalInformation = 79,
        Tanker = 80,
        TankerHazardousCategoryA = 81,
        TankerHazardousCategoryB = 82,
        TankerHazardousCategoryC = 83,
        TankerHazardousCategoryD = 84,
        TankerReserved1 = 85,
        TankerReserved2 = 86,
        TankerReserved3 = 87,
        TankerReserved4 = 88,
        TankerNoAdditionalInformation = 89,
        OtherType = 90,
        OtherTypeHazardousCategoryA = 91,
        OtherTypeHazardousCategoryB = 92,
        OtherTypeHazardousCategoryC = 93,
        OtherTypeHazardousCategoryD = 94,
        OtherTypeReserved1 = 95,
        OtherTypeReserved2 = 96,
        OtherTypeReserved3 = 97,
        OtherTypeReserved4 = 98,
        OtherTypeNoAdditionalInformation = 99,
    }
}