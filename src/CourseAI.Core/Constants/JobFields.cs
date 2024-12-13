namespace CourseAI.Core.Constants;

public static class JobFields
{
    #region Import Fields

    public const string DoNumber = "doNumber"; // Delivery Order Number
    public const string DeliveryDate = "deliveryDate";
    public const string Address = "address";
    public const string Address1 = "address1";
    public const string Address2 = "address2";
    public const string Address3 = "address3";
    public const string PostalCode = "postalCode";
    public const string City = "city";
    public const string State = "state";
    public const string Country = "country";
    public const string CompanyName = "companyName";
    public const string DeliverToCollectFrom = "deliverToCollectFrom";
    public const string LastName = "lastName";
    public const string PhoneNumber = "phoneNumber";
    public const string NotifyEmail = "notifyEmail";
    public const string SenderPhoneNumber = "senderPhoneNumber";
    public const string FaxNumber = "faxNumber";
    public const string Group = "group";
    public const string DetrackJobType = "detrackJobType";
    public const string PrimaryJobStatus = "primaryJobStatus";
    public const string AssignTo = "vehicle.name";
    public const string AutoReschedule = "autoReschedule";
    public const string JobType = "jobType";
    public const string JobTime = "jobTime";
    public const string TimeWindow = "timeWindow";
    public const string CompletionTimeWindowFrom = "completionTimeWindowFrom";
    public const string CompletionTimeWindowTo = "completionTimeWindowTo";
    public const string EtaTime = "etaTime";
    public const string Instructions = "instructions";
    public const string NumberOfShippingLabels = "numberOfShippingLabels";
    public const string AttachmentUrl = "attachmentUrl";
    public const string JobSequence = "jobSequence";
    public const string RunNumber = "runNumber";
    public const string TrackingNumber = "trackingNumber";
    public const string OrderNumber = "orderNumber";
    public const string StartDate = "startDate";
    public const string DeliveryCompletionDate = "deliveryCompletionDate";
    public const string JobReceivedDate = "jobReceivedDate";
    public const string Zone = "zone";
    public const string AddressLatitude = "addressLatitude";
    public const string AddressLongitude = "addressLongitude";
    public const string JobReleaseTime = "jobReleaseTime";
    public const string OpenToMarketplace = "openToMarketplace";
    public const string MarketplaceOffer = "marketplaceOffer";
    public const string JobFee = "jobFee";
    public const string JobPrice = "jobPrice";
    public const string Customer = "customer";
    public const string AccountNumber = "accountNumber";
    public const string BillingAddress = "billingAddress";
    public const string JobOwner = "jobOwner";
    public const string InvoiceNumber = "invoiceNumber";
    public const string InvoiceAmount = "invoiceAmount";
    public const string PaymentMode = "paymentMode";
    public const string PaymentAmount = "paymentAmount";
    public const string TimeZone = "timeZone";
    public const string Remarks = "remarks";
    public const string Weight = "weight";
    public const string ParcelWidth = "parcelWidth";
    public const string ParcelLength = "parcelLength";
    public const string ParcelHeight = "parcelHeight";
    public const string Cbm = "cbm";
    public const string Boxes = "boxes";
    public const string Cartons = "cartons";
    public const string Pieces = "pieces";
    public const string Envelopes = "envelopes";
    public const string Pallets = "pallets";
    public const string Bins = "bins";
    public const string Trays = "trays";
    public const string Bundles = "bundles";
    public const string Rolls = "rolls";
    public const string Source = "source";
    public const string WebhookUrl = "webhookUrl";
    public const string Carrier = "carrier";
    public const string Depot = "depot";
    public const string DepotContact = "depotContact";
    public const string DepotContactNumber = "depotContactNumber";
    public const string DepotAddress = "depotAddress";
    public const string Department = "department";
    public const string SalesPerson = "salesPerson";
    public const string IdentificationNumber = "identificationNumber";
    public const string BankPrefix = "bankPrefix";
    public const string InsurancePrice = "insurancePrice";
    public const string InsuranceCoverage = "insuranceCoverage";
    public const string TotalPrice = "totalPrice";
    public const string PayerType = "payerType";
    public const string ServiceType = "serviceType";
    public const string WarehouseAddress = "warehouseAddress";
    public const string DestinationTimeWindow = "destinationTimeWindow";
    public const string Door = "door";
    public const string VehicleType = "vehicleType";
    public const string ServiceTime = "serviceTime";
    public const string Priority = "priority";
    public const string OtherPhoneNumbers = "otherPhoneNumbers";
    public const string CustomImportField1 = "custom1";
    public const string CustomImportField2 = "custom2";
    public const string CustomImportField3 = "custom3";
    public const string CustomImportField4 = "custom4";
    public const string CustomImportField5 = "custom5";

    #endregion

    #region Import Item Fields

    public const string Sku = "sku";
    public const string ItemPoNumber = "itemPoNumber";
    public const string ItemBatchNumber = "itemBatchNumber";
    public const string ExpiryDate = "expiryDate";
    public const string ItemDescription = "itemDescription";
    public const string Quantity = "quantity";
    public const string UnitOfMeasure = "unitOfMeasure";
    public const string ItemWeight = "itemWeight";
    public const string Comments = "comments";
    public const string CustomImportItemField1 = "customImport1";
    public const string CustomImportItemField2 = "customImport2";
    public const string CustomImportItemField3 = "customImport3";
    public const string CustomImportItemField4 = "customImport4";
    public const string CustomImportItemField5 = "customImport5";

    #endregion

    #region Export Fields

    public const string JobAge = "jobAge";
    public const string DetrackNumber = "detrackNumber";
    public const string JobStatus = "jobStatus";
    public const string TrackingStatus = "trackingStatus";
    public const string PodTime = "podTime";
    public const string Reason = "reason";
    public const string Attempt = "attempt";
    public const string ReceivedBy = "receivedBy";
    public const string Note = "note";
    public const string PodLatitude = "podLatitude";
    public const string PodLongitude = "podLongitude";
    public const string PodAddress = "podAddress";
    public const string PodAt = "podAt";
    public const string AddressTrackedAt = "addressTrackedAt";
    public const string ArrivedLatitude = "arrivedLatitude";
    public const string ArrivedLongitude = "arrivedLongitude";
    public const string ArrivedAddress = "arrivedAddress";
    public const string ArrivedAt = "arrivedAt";
    public const string TextedAt = "textedAt";
    public const string CalledAt = "calledAt";
    public const string SerialNumber = "serialNumber";
    public const string SignedAt = "signedAt";
    public const string Photo1At = "photo1At";
    public const string Photo2At = "photo2At";
    public const string Photo3At = "photo3At";
    public const string Photo4At = "photo4At";
    public const string Photo5At = "photo5At";
    public const string Photo6At = "photo6At";
    public const string Photo7At = "photo7At";
    public const string Photo8At = "photo8At";
    public const string Photo9At = "photo9At";
    public const string Photo10At = "photo10At";
    public const string ActualWeight = "actualWeight";
    public const string Temperature = "temperature";
    public const string HoldTime = "holdTime";
    public const string PaymentCollected = "paymentCollected";
    public const string ItemsCount = "itemsCount";
    public const string ActualCrates = "actualCrates";
    public const string ActualPallets = "actualPallets";
    public const string ActualUtilization = "actualUtilization";
    public const string GoodsServiceRating = "goodsServiceRating";
    public const string DriverRating = "driverRating";
    public const string CustomerFeedback = "customerFeedback";
    public const string LiveEta = "liveEta";
    public const string HeadToDeliveryAt = "headToDeliveryAt";
    public const string MassPodSubmission = "massPodSubmission";
    public const string VehicleGpsStatus = "vehicleGpsStatus";
    public const string VehicleGpsPermission = "vehicleGpsPermission";
    public const string OutOfGeofenceWarningAcknowledgedAt = "outOfGeofenceWarningAcknowledgedAt";
    public const string GeofenceLatitude = "geofenceLatitude";
    public const string GeofenceLongitude = "geofenceLongitude";
    public const string TrackingLink = "trackingLink";
    public const string CustomExportField1 = "customExport1";
    public const string CustomExportField2 = "customExport2";
    public const string CustomExportField3 = "customExport3";
    public const string CustomExportField4 = "customExport4";
    public const string CustomExportField5 = "customExport5";

    #endregion

    #region Export Item Fields

    public const string ActualQuantity = "actualQuantity";
    public const string RejectQuantity = "rejectQuantity";
    public const string ItemRejectReason = "itemRejectReason";
    public const string ItemSerialNumber = "itemSerialNumber";
    public const string ItemChecked = "itemChecked";
    public const string UnloadTimeEstimate = "unloadTimeEstimate";
    public const string UnloadTimeActual = "unloadTimeActual";
    public const string FollowUpQuantity = "followUpQuantity";
    public const string FollowUpReason = "followUpReason";
    public const string ReworkQuantity = "reworkQuantity";
    public const string ReworkReason = "reworkReason";
    public const string InboundQuantity = "inboundQuantity";
    public const string PhotoUrl = "photoUrl";
    public const string CustomExportItemField1 = "customExportItem1";
    public const string CustomExportItemField2 = "customExportItem2";
    public const string CustomExportItemField3 = "customExportItem3";
    public const string CustomExportItemField4 = "customExportItem4";
    public const string CustomExportItemField5 = "customExportItem5";

    #endregion
}