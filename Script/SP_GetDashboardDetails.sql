USE [VendorManagementDB2]
GO

/****** Object:  StoredProcedure [dbo].[GetDashboardDetails]    Script Date: 7/31/2024 1:57:56 AM ******/
DROP PROCEDURE [dbo].[GetDashboardDetails]
GO

/****** Object:  StoredProcedure [dbo].[GetDashboardDetails]    Script Date: 7/31/2024 1:57:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetDashboardDetails]
    @DetailsYear NVARCHAR(10)
AS
BEGIN
    SELECT 
        COUNT(DISTINCT v.VendorName) AS CountOfVendors,
        COUNT(DISTINCT vs.VendorServiceName) AS CountOfServices,
        SUM(ISNULL(vd.ServiceSantionAmount, 0)) AS TotalSanctionAmount,
        SUM(ISNULL(vp.VendorPaymentAmount, 0)) AS TotalPaidAmount
    FROM 
        [Vendors] AS v
    INNER JOIN 
        [VendorServices] AS vs ON v.VendorID = vs.FK_VendorID
    INNER JOIN 
        [VendorDetails] AS vd ON vd.FK_VendorServiceID = vs.VendorServiceID 
    LEFT JOIN 
        [VendorPayments] AS vp ON vp.FK_VendorDetailID = vd.VendorDetailID
    WHERE 
        vd.DetailsYear = @DetailsYear
END
GO


