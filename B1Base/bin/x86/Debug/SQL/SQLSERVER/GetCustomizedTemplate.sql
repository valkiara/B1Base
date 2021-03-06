﻿select  "Left", 
		"Top", 
		"Editable",
		"Visible"
from UIC2
inner join UICU on (UICU."TPLId" = UIC2."TPLId" and UICU."UserID" = {0})
where UIC2."FormId" = '{1}'
and UIC2."ItemId" = '{2}'
and ("Top" > 0 or "Left" > 0)
and {3} = 0
and UICU."IsTemplate" = 'N'
union
select  "Left", 
		"Top", 
		"Editable",
		"Visible"
from UIC2
inner join UICU on (UICU."TPLId" = UIC2."TPLId")
inner join UIC3 on (UIC3."TPLId" = UICU."TPLId" and UIC3."UserID" = {0})
where UIC2."FormId" = '{1}'
and UIC2."ItemId" = '{2}'
and ("Top" > 0 or "Left" > 0)
and {3} = 0
and UICU."IsTemplate" = 'Y'
union
select  "Left", 
		"Top", 
		"Editable",
		"Visible"
from UIC2
inner join UICU on (UICU."TPLId" = UIC2."TPLId")
where UIC2."FormId" = '{1}'
and UIC2."ItemId" = '{2}'
and UIC2."TPLId" = {3}
and UICU."IsTemplate" = 'Y'