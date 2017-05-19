<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubmitTicket.ascx.cs" Inherits="TicketDesk.ServiceClient.SubmitTicket" %>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>

<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
    type="text/javascript"></script>

<form runat="server">
    <asp:Panel ID="FormPanel" CssClass="div_formpanel" runat="server" Width="920px">
        <div style="padding: 20px; border: solid 1px #ddd;">
            <table style="font-family: 'Lato', sans-serif; width: 100%">
                <tr>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Emri i plote
                        </label>
                        <div id="div_contact_invul">
                            <asp:TextBox ID="txtFullName" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="txtFullName"
                            ErrorMessage="* Ju lutem plotesoni emrin dhe mbiemrin tuaj!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            E-mail
                        </label>
                        <div id="div4">
                            <asp:TextBox ID="txtClientEmail" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="test" EnableClientScript="true" ForeColor="#E11419" ControlToValidate="txtClientEmail"
                            Display="Dynamic" runat="server" ErrorMessage="* Ju lutem plotesoni email tuaj!"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" EnableClientScript="true" ValidationGroup="test" runat="server" ControlToValidate="txtClientEmail"
                            Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ErrorMessage="* Adresa juaj e email nuk eshte e sakte!" ForeColor="#E11419"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Numri i tel
                        </label>
                        <div id="div_contact_invul">
                            <asp:TextBox ID="txtClientPhone" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="txtClientPhone"
                            ErrorMessage="* Ju lutem plotesoni numrin e tel!" Display="Dynamic" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" EnableClientScript="true" ValidationGroup="test" runat="server" ControlToValidate="txtClientPhone"
                            ErrorMessage="* Ju lutem vendosni vetem numra!" Display="Dynamic" ForeColor="#E11419" ValidationExpression="^\d+$">
                        </asp:RegularExpressionValidator>
                    </td>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Adresa
                        </label>
                        <div id="div_contact_invul">
                            <asp:TextBox ID="txtAddress" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="txtAddress"
                            ErrorMessage="* Ju lutem plotesoni adresen!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Tipi kerkeses
                        </label>
                        <div id="div_contact_invul">
                            <asp:DropDownList ID="ddlTicketType" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;">
                                <asp:ListItem Text="--Zgjidh--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Instalim" Value="Instalim"></asp:ListItem>
                                <asp:ListItem Text="Konfigurim" Value="Konfigurim"></asp:ListItem>
                                <asp:ListItem Text="Mirembajtje" Value="Mirembajtje"></asp:ListItem>
                                <asp:ListItem Text="Difekt" Value="Difekt"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="ddlTicketType"
                            ErrorMessage="* Ju lutem zgjidhni tipin e kerkeses!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Kategoria
                        </label>
                        <div id="div_contact_invul">
                            <asp:DropDownList ID="ddlCategory" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;">
                                <asp:ListItem Text="--Zgjidh--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="TV" Value="TV"></asp:ListItem>
                                <asp:ListItem Text="Kamera" Value="Kamera"></asp:ListItem>
                                <asp:ListItem Text="Brava" Value="Brava"></asp:ListItem>
                                <asp:ListItem Text="Internet" Value="Internet"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="ddlCategory"
                            ErrorMessage="* Ju lutem zgjidhni kategorine e kerkeses!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Emri kerkeses
                        </label>
                        <div id="div_contact_invul">
                            <asp:TextBox ID="txtTicketName" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="txtTicketName"
                            ErrorMessage="* Ju lutem plotesoni emrin e kerkeses!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Prioriteti
                        </label>
                        <div id="div_contact_invul">
                            <asp:DropDownList ID="ddlPriority" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;">
                                <asp:ListItem Text="--Zgjidh--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Ulet" Value="Ulet"></asp:ListItem>
                                <asp:ListItem Text="Mesem" Value="Mesem"></asp:ListItem>
                                <asp:ListItem Text="Larte" Value="Larte"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="test" EnableClientScript="true" runat="server" ControlToValidate="ddlPriority"
                            ErrorMessage="* Ju lutem zgjidhni prioritetin e kerkeses!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%" valign="top">
                        <div id="div_contact_invul">
                            <asp:CheckBox ID="chkAffCos" runat="server" Style="font-family: 'Lato', sans-serif;" Text="Ka difekt" />
                        </div>
                    </td>
                    <td style="width: 50%">
                        <label class="div_contact_item" style="width: 350px;">
                            Detajet e kerkeses</label>

                        <div id="div5" style="top: 151px; width: 300px;">
                            <asp:TextBox TextMode="MultiLine" ID="txtTicketDetails" Width="303" Height="150" runat="server"
                                Style="border: 1px solid #dbd5d9; min-height: 150px; max-height: 15px; max-width: 303px; min-width: 300px;"
                                CssClass="contact_data"></asp:TextBox><br />
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="test" runat="server" EnableClientScript="true" ControlToValidate="txtTicketDetails"
                            ErrorMessage="* Ju lutem shkruani detajet e kerkeses tuaj!" ForeColor="#E11419">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>

                    <td style="width: 50%">
                        <label class="div_contact_item">
                            Pyetje sigurie: Sa shkronja ka ArfaNet</label>
                        <div id="div6">
                            <asp:TextBox ID="txtAnswer" runat="server" Width="300px" Height="30px" Style="border: 1px solid #dbd5d9!important;"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="#E11419" ValidationGroup="test" EnableClientScript="true" ControlToValidate="txtAnswer"
                            Display="Dynamic" runat="server" ErrorMessage="* Ju lutem plotesoni pyetjen e sigurise!"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 50%;" align="right">
                        <div id="div_nextpage">
                            <asp:Button ID="SendButton" runat="server" Text="Dergo" ValidationGroup="test" CausesValidation="true" Style="cursor: pointer; padding: 10px 15px; border: none!important; background: #E11419; color: #ffffff; font-family: 'Lato', sans-serif;" OnClick="SendButton_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="ResponsePanel" CssClass="div_antwoordpanel" Width="920px" runat="server"
        Visible="false">
        <div style="padding: 20px; width: 920px; border: solid 1px #ddd;">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </asp:Panel>
</form>

