select Manufacture.M_Name
from Production_Plan inner join Manufacture on Production_Plan.Id = Manufacture.Production_Task_Id
where Manufacture.M_Name is not null
group by Manufacture.M_Name