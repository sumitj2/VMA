USE [VendorManagementDB2]
GO

/****** Object:  StoredProcedure [dbo].[GetYearlyAllServiceReport]    Script Date: 7/31/2024 1:58:49 AM ******/
DROP PROCEDURE [dbo].[GetYearlyAllServiceReport]
GO

/****** Object:  StoredProcedure [dbo].[GetYearlyAllServiceReport]    Script Date: 7/31/2024 1:58:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetYearlyAllServiceReport]
    @DetailsYear NVARCHAR(10)
AS
BEGIN
    SELECT 
        v.VendorName, 
        vs.VendorServiceName, 
        vd.ServicePaymentType,
        vd.ServiceSantionAmount,
        CASE 
            WHEN vd.ServicePaymentType = 'Monthly' THEN 12
            WHEN vd.ServicePaymentType = 'Quarterly' THEN 4
            WHEN vd.ServicePaymentType = 'Half Yearly' THEN 2
            WHEN vd.ServicePaymentType = 'Yearly' THEN 1
            ELSE 1 -- For 'None' or any other unexpected value
        END AS NumberOfTerms,
        COUNT(vp.VendorPaymentID) AS TotalPaymentsMade,
        CASE 
            WHEN vd.ServicePaymentType = 'Monthly' THEN 12
            WHEN vd.ServicePaymentType = 'Quarterly' THEN 4
            WHEN vd.ServicePaymentType = 'Half Yearly' THEN 2
            WHEN vd.ServicePaymentType = 'Yearly' THEN 1
            ELSE 1 -- For 'None' or any other unexpected value
        END - COUNT(vp.VendorPaymentID) AS RemainingTerms,
        SUM(vp.VendorPaymentAmount) AS TotalVendorPaymentAmount,
        vd.ServiceSantionAmount - SUM(ISNULL(vp.VendorPaymentAmount, 0)) AS RemainingAmount
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
    GROUP BY 
        v.VendorName, 
        vs.VendorServiceName, 
        vd.ServicePaymentType,
        vd.ServiceSantionAmount
END
GO


