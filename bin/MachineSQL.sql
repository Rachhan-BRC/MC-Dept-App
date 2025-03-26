
/*
--CurrentStock
SELECT Code, ItemName, RMTypeName, DocumentNo,
CASE 
	WHEN MCName1 IS NOT NULL AND MCName2 IS NULL THEN MCName1 
	WHEN MCName1 IS NULL AND MCName2 IS NOT NULL THEN MCName2 
	ELSE NULL 
END AS MCName, TotalQty, 0 AS POS, 0.00 AS Semi, 0 AS SD, 0.00 AS NG FROM 
(SELECT Code, POSNo AS DocumentNo, ROUND(SUM(StockValue),0) AS TotalQty FROM tbSDMCAllTransaction WHERE CancelStatus=0 AND LocCode='MC1' GROUP BY Code, POSNo) T1 
LEFT JOIN (SELECT SysNo, MCName AS MCName1 FROM tbSDAllocateStock GROUP BY SysNo, MCName) T2 ON T1.DocumentNo=T2.SysNo 
LEFT JOIN (SELECT PosCNo,
NULLIF(CONCAT(MC1Name, 
CASE 
	WHEN LEN(MC2Name)>1 THEN ' & ' 
	ELSE '' 
END, MC2Name, 
CASE 
	WHEN LEN(MC3Name)>1 THEN ' & ' 
	ELSE '' 
END, MC3Name),'') AS MCName2 FROM tbPOSDetailofMC) T3 ON T1.DocumentNo = T3.PosCNo 
LEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T1.Code=T4.ItemCode
WHERE TotalQty <> 0 AND (TRIM(DocumentNo)='' OR DocumentNo IS NULL)
ORDER BY Code ASC, DocumentNo ASC
*/

--POS Detail
SELECT SubLoc, LabelNo, ItemCode, QtyDetails AS DocumentNo, Qty FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND CountingMethod='POS' 

--POS
/*
SELECT ItemCode, DocumentNo, SUM(Qty) AS TotalQty FROM
(SELECT SubLoc, LabelNo, ItemCode, QtyDetails AS DocumentNo, Qty FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND CountingMethod='POS' ) T1
GROUP BY ItemCode, DocumentNo
*/

--SemiDetail 

SELECT SubLoc, LabelNo, T6.ItemName AS WIPName, Qty AS WIPQty,
CASE 
	WHEN T5.Code IS NULL THEN T1.POSNo
	ELSE T3.SysNo
END AS DocumentNo, T1.POSNo, T2.LowItemCode, T4.ItemName, (T2.LowQty*Qty) As TotalQty FROM 
(SELECT SubLoc, LabelNo, LEFT(QtyDetails2, CHARINDEX('|', QtyDetails2) - 1) AS POSNo, ItemCode, Qty FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND CountingMethod = 'Semi') T1 
INNER JOIN (SELECT * FROM MstBOM) T2 ON T1.ItemCode=T2.UpItemCode 
LEFT JOIN (SELECT SysNo, POSNo FROM tbSDAllocateStock INNER JOIN (SELECT SD_DocNo FROM tbBobbinRecords WHERE In_Date IS NULL GROUP BY SD_DocNo)T3_1 ON tbSDAllocateStock.SysNo=T3_1.SD_DocNo) T3 ON T1.POSNo=T3.POSNo 
INNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T2.LowItemCode = T4.ItemCode 
LEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T5 ON T2.LowItemCode = T5.Code
LEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') T6 ON T1.ItemCode=T6.ItemCode
--Semi
/*
SELECT LowItemCode, DocumentNo, ROUND(SUM(TotalQty),2) AS TotalQty FROM 
(SELECT SubLoc, LabelNo, T6.ItemName AS WIPName, Qty AS WIPQty,
CASE 
	WHEN T5.Code IS NULL THEN T1.POSNo
	ELSE T3.SysNo
END AS DocumentNo, T1.POSNo, T2.LowItemCode, T4.ItemName, (T2.LowQty*Qty) As TotalQty FROM 
(SELECT SubLoc, LabelNo, LEFT(QtyDetails2, CHARINDEX('|', QtyDetails2) - 1) AS POSNo, ItemCode, Qty FROM tbInventory WHERE LocCode='MC1' AND CancelStatus = 0 AND CountingMethod = 'Semi') T1 
INNER JOIN (SELECT * FROM MstBOM) T2 ON T1.ItemCode=T2.UpItemCode 
LEFT JOIN (SELECT SysNo, POSNo FROM tbSDAllocateStock INNER JOIN (SELECT SD_DocNo FROM tbBobbinRecords WHERE In_Date IS NULL GROUP BY SD_DocNo)T3_1 ON tbSDAllocateStock.SysNo=T3_1.SD_DocNo) T3 ON T1.POSNo=T3.POSNo 
INNER JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Material') T4 ON T2.LowItemCode = T4.ItemCode 
LEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T5 ON T2.LowItemCode = T5.Code
LEFT JOIN (SELECT * FROM tbMasterItem WHERE ItemType='Work In Process') T6 ON T1.ItemCode=T6.ItemCode) TbSemi
GROUP BY LowItemCode, DocumentNo
ORDER BY DocumentNo ASC, LowItemCode ASC
*/

--SDDetails
SELECT SubLoc, LabelNo, QtyDetails AS DocumentNo, ItemCode, Qty FROM tbInventory 
WHERE LocCode = 'MC1' AND CancelStatus = 0 AND CountingMethod = 'SD Document'

--SD
/*
SELECT ItemCode, DocumentNo, SUM(Qty) AS TotalQty FROM
(SELECT SubLoc, LabelNo, QtyDetails AS DocumentNo, ItemCode, Qty FROM tbInventory 
WHERE LocCode = 'MC1' AND CancelStatus = 0 AND CountingMethod = 'SD Document') T1
GROUP BY ItemCode, DocumentNo
ORDER BY ItemCode ASC, DocumentNo ASC
*/

--NG Details


--NG Details
SELECT MCSeqNo, 
CASE 
	WHEN Code IS NULL THEN PosCNo
	ELSE SDNo
END AS DocumentNo, POSNo, RMCode, Qty FROM tbNGInprocess 
LEFT JOIN (SELECT SysNo AS SDNo, T1.POSNo  FROM
	(SELECT *  FROM tbSDAllocateStock) T1 INNER JOIN (SELECT POSNo, MIN(RegDate) AS RegDate FROM tbSDAllocateStock GROUP BY POSNo) T2 ON T1.POSNo=T2.POSNo AND T1.RegDate=T2.RegDate) TbSDAlloc ON tbNGInprocess.PosCNo=TbSDAlloc.POSNo
LEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T3 ON RMCode = T3.Code
WHERE ReqStatus = 0 AND Qty<>0

--NG
/*
SELECT RMCode, DocumentNo, SUM(Qty) AS TotalQty FROM
(
SELECT MCSeqNo, 
CASE 
	WHEN Code IS NULL THEN PosCNo
	ELSE SDNo
END AS DocumentNo, POSNo, RMCode, Qty FROM tbNGInprocess 
LEFT JOIN (SELECT SysNo AS SDNo, T1.POSNo  FROM
	(SELECT *  FROM tbSDAllocateStock) T1 INNER JOIN (SELECT POSNo, MIN(RegDate) AS RegDate FROM tbSDAllocateStock GROUP BY POSNo) T2 ON T1.POSNo=T2.POSNo AND T1.RegDate=T2.RegDate) TbSDAlloc ON tbNGInprocess.PosCNo=TbSDAlloc.POSNo
LEFT JOIN (SELECT Code FROM tbSDMCAllTransaction WHERE CancelStatus = 0 AND LocCode = 'MC1' AND ReceiveQty>0 AND POSNo LIKE 'SD%' GROUP BY Code) T3 ON RMCode = T3.Code
WHERE ReqStatus = 0 AND Qty<>0
) tbNG
GROUP BY RMCode, DocumentNo
ORDER BY RMCode ASC, DocumentNo ASC


*/