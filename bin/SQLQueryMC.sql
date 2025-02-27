CREATE TABLE tbMstRMRegister(
	BobbinSysNo  nvarchar (25) NOT NULL, 
	RMCode nvarchar (25) NOT NULL, 
	RMType nvarchar (100) NOT NULL, 
	BobbinW float NOT NULL, 
	MOQ_W float NOT NULL, 
	MOQ_L float NOT NULL, 
	Remain_W float NOT NULL, 
	Remain_L float NOT NULL, 
	Per_Unit float NOT NULL,
	C_Location nvarchar (50) NOT NULL, 	
	Status nvarchar (50) NOT NULL, 
	RegDate datetime NOT NULL, 
	RegBy nvarchar (100) COLLATE Khmer_100_CI_AI_SC_UTF8 NOT NULL, 
	UpdateDate datetime NOT NULL, 
	UpdateBy nvarchar (100) COLLATE Khmer_100_CI_AI_SC_UTF8 NOT NULL 

) 
CREATE TABLE tbBobbinRecords(
	BobbinSysNo  nvarchar (25) NOT NULL, 	
	RMCode nvarchar (25) NOT NULL, 
	BStock_Kg float NOT NULL,
	BStock_QTY float NOT NULL,
	AStock_Kg float NULL,
	AStock_QTY float NULL,
	Used_Qty float NULL,
	MCName nvarchar (200) NOT NULL,
	SD_DocNo nvarchar (50) NOT NULL,
	In_Date datetime NULL,
	Out_Date datetime NULL,
)


SELECT BobbinSysNo, Remain_W, Remain_L FROM tbMstRMRegister 
WHERE C_Location='WIR1' AND Remain_L>0 AND RMCode='1146' 
ORDER BY Remain_L ASC

SELECT ItemCode, ItemName, RMType, R2OrBobbinsW, R1OrNetW, MOQ, BobbinsOrReil FROM
(SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T1
LEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 ON T1.ItemCode = T2.Code
WHERE ItemCode = '3427'


SELECT MAX(BobbinSysNo) AS BobbinSysNo FROM tbMstRMRegister WHERE RMType = 'Wire'

