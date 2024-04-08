using MauiPopup;
using MauiPopup.Views;

namespace StarBank.PopUp;

public partial class Servicios : BasePopupPage
{
    public Servicios()
    {
        InitializeComponent();
    }

    private void Button_Clicked3(object sender, EventArgs e)
    {
        PopupAction.ClosePopup(this);
    }
}