namespace Domain.Enums
{
    public enum StatusOrder
    {
        CONFIRM = 0,  //chờ xác nhận
        PENDING_SHIP = 1,  //chuẩn bị hàng
        SHIPPING = 2, //đang giao hàng
        DELIVERIED = 3,  //thành công
        CANCELED = 4  //hủy
    }
}