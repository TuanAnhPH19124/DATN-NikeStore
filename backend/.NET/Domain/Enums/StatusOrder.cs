namespace Domain.Enums
{
    public enum StatusOrder
    {
        CONFIRM = 0,  //đã thanh toán
        PENDING_SHIP = 1,  //chuẩn bị ship
        SHIPPING = 2, //đang ship
        DELIVERIED = 3,  //thành công
        DECLINE = 4  //hủy
    }
}