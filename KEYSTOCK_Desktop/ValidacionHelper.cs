using System;
using System.Linq; // Necesario para usar .Contains()
using System.Windows.Forms;

namespace KEYSTOCK_Desktop // Ajusta a tu namespace
{
    public static class ValidacionHelper
    {
        // Agregamos "params Control[] excepciones" al final
        public static bool FormularioEsValido(Control.ControlCollection controles, params Control[] excepciones)
        {
            foreach (Control control in controles)
            {
                // Ignorar controles ocultos o deshabilitados
                if (!control.Visible || !control.Enabled) continue;

                // NUEVO: Ignorar el control si fue enviado en la lista de excepciones
                if (excepciones != null && excepciones.Contains(control)) continue;

                if (control is TextBox txt && string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show($"El campo {txt.Name.Replace("txt", "")} es obligatorio.",
                                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt.Focus();
                    return false;
                }

                if (control is ComboBox cmb && cmb.SelectedIndex == -1)
                {
                    MessageBox.Show($"Debe seleccionar una opción válida en {cmb.Name.Replace("cmb", "")}.",
                                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmb.Focus();
                    return false;
                }

                // Recursividad para Paneles y GroupBoxes (pasando las excepciones hacia abajo)
                if (control.HasChildren)
                {
                    if (!FormularioEsValido(control.Controls, excepciones))
                        return false;
                }
            }
            return true;
        }
    }
}