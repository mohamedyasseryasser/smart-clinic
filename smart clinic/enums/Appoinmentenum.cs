namespace smart_clinic.enums
{
    public enum AppointmentStatus
    {
        Pending,     // لسه لم يتم التأكيد
        Confirmed,   // تم التأكيد
        Completed,   // انتهت الزيارة
        Cancelled,   // تم الإلغاء
        NoShow       // المريض لم يحضر
    }
}
