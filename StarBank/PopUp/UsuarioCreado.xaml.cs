using MauiPopup.Views;
using MauiPopup;
namespace StarBank.PopUp;

public partial class UsuarioCreado : BasePopupPage
{
    public UsuarioCreado()
    {
        InitializeComponent();
    }

    private void Button_Clicked5(object sender, EventArgs e)
    {
        PopupAction.ClosePopup(this);

    }
}