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
	Resv1 nvarchar(5) NULL,	
	Resv2 nvarchar(5) NULL,	
	Resv3 nvarchar(5) NULL, 
	Resv4 nvarchar(5) NULL, 
	Resv5 nvarchar(5) NULL, 
	RegDate datetime NOT NULL, 
	RegBy nvarchar (100) COLLATE Khmer_100_CI_AI_SC_UTF8 NOT NULL, 
	UpdateDate datetime NOT NULL, 
	UpdateBy nvarchar (100) COLLATE Khmer_100_CI_AI_SC_UTF8 NOT NULL 
) 


SELECT ItemCode, ItemName, RMType, R2OrBobbinsW, R1OrNetW, MOQ, BobbinsOrReil FROM
(SELECT * FROM tbMasterItem WHERE ItemType = 'Material') T1
LEFT JOIN (SELECT * FROM tbSDMstUncountMat) T2 ON T1.ItemCode = T2.Code
WHERE ItemCode = '3427'


SELECT MAX(BobbinSysNo) AS BobbinSysNo FROM tbMstRMRegister WHERE RMType = 'Wire'

