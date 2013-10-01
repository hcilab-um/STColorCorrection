import javax.swing.*;
import javax.swing.border.BevelBorder;
import javax.swing.border.CompoundBorder;
import javax.swing.border.LineBorder;
import javax.swing.border.MatteBorder;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.Hashtable;
import java.util.Vector;

/**
 * Color Mixing alghoritms
 * User: alberto
 * Date: 29/01/13
 * Time: 21:28
 */
public class ColorMix {

    Vector<JLabel> firstMixColors;
    Vector<JLabel> secondMixColors;
    JComboBox/*<Mixer>*/ comboBox;
    JLabel firstMixColor;
    JLabel firstSel;
    JLabel secondSel;
    JLabel finalColor;

    public ColorMix() {
        firstMixColors = new Vector<JLabel>();
        Vector<Mixer> mixers = new Vector<Mixer>();
        mixers.add(new AdditiveMixer());
        mixers.add(new SustractiveMixer());
        mixers.add(new TertiaryMixer());
        mixers.add(new DilutingSustractiveMixer());

        comboBox = new JComboBox(new DefaultComboBoxModel(mixers));
        firstMixColor = buildColorLabel();
        firstSel = buildColorLabel();
        secondSel = buildColorLabel();
        secondMixColors = new Vector<JLabel>();
        secondMixColors.add(firstSel);
        secondMixColors.add(secondSel);
        finalColor = buildColorLabel();
        comboBox.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                calculateMixes();
            }
        });
        buildGUI();
    }

    private JLabel buildColorLabel() {
        JLabel label = new JLabel();
        label.setOpaque(true);
        label.setHorizontalAlignment(SwingConstants.CENTER);
        label.setHorizontalTextPosition(SwingConstants.CENTER);
        label.setBorder(BorderFactory.createLineBorder(Color.BLACK));
        label.setPreferredSize(new Dimension(100,25));
        return label;
    }

    public void buildGUI() {
        JFrame frame = new JFrame();
        frame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        frame.setTitle("Mixing colors");

        frame.setLayout(new GridBagLayout());
        GridBagConstraints cc = new GridBagConstraints();
        cc.fill = GridBagConstraints.BOTH;
        cc.insets = new Insets(5, 5, 5, 5);
        cc.weightx = .2;
        cc.weighty = 1;
        frame.getContentPane().add(buildColorPanel(0), cc);
        frame.getContentPane().add(buildColorPanel(1), cc);
        cc.gridy = 1;
        JPanel firstMix = new JPanel(new GridBagLayout());
        GridBagConstraints ccCol = new GridBagConstraints();
        ccCol.fill = GridBagConstraints.BOTH;
        ccCol.insets = new Insets(5, 5, 5, 5);
        ccCol.weightx = 1;
        ccCol.weighty = 1;

        ccCol.gridx = 0;
        ccCol.gridy = 0;
        ccCol.gridheight = 2;
        firstMix.add(firstMixColor, ccCol);
        ccCol.fill = GridBagConstraints.HORIZONTAL;
        ccCol.weightx = 0.2;
        ccCol.weighty = 0.5;
        ccCol.gridx = 1;
        ccCol.gridy = 0;
        ccCol.gridheight = 1;
        ccCol.gridwidth = 1;
        firstMix.add(new JButton(new AbstractAction("Set First") {
            @Override
            public void actionPerformed(ActionEvent e) {
                setBackgroundToLabel(firstSel, firstMixColor.getBackground());
                calculateMixes();
            }
        }), ccCol);
        ccCol.gridx = 1;
        ccCol.gridy = 1;
        firstMix.add(new JButton(new AbstractAction("Set Second") {
            @Override
            public void actionPerformed(ActionEvent e) {
                setBackgroundToLabel(secondSel, firstMixColor.getBackground());
                calculateMixes();
            }
        }), ccCol);
        firstMix.setBorder(BorderFactory.createTitledBorder("Secondary Colors"));
        frame.getContentPane().add(firstMix, cc);
        cc.weightx = .6;

        JPanel panel = new JPanel(new GridBagLayout());
        GridBagConstraints ccColor = new GridBagConstraints();
        ccColor.fill = GridBagConstraints.BOTH;
        ccColor.insets = new Insets(5, 5, 5, 5);
        ccColor.weightx = 1;
        ccColor.weighty = 1;
        panel.add(firstSel, ccColor);
        ccColor.gridx = 1;
        panel.add(secondSel, ccColor);
        ccColor.gridx = 0;
        ccColor.gridy = 1;
        ccColor.weighty = 0;
        ccColor.gridwidth = 2;
        panel.add(finalColor, ccColor);
        ccColor.gridy = 2;
        panel.add(comboBox, ccColor);
        panel.setBorder(BorderFactory.createTitledBorder("Tertiary Colors"));
        frame.getContentPane().add(panel, cc);
        frame.pack();
        frame.setVisible(true);
    }


    public static void main(String[] args) {
        new ColorMix();
    }

    private JComponent buildColorPanel(int selectedIndex) {
        final JLabel pColor = buildColorLabel();
        firstMixColors.add(pColor);
        JPanel pSelectColor = new JPanel(new GridBagLayout());
        GridBagConstraints cc = new GridBagConstraints();
        cc.fill = GridBagConstraints.BOTH;
        cc.insets = new Insets(5, 5, 5, 5);
        cc.weightx = 1;
        cc.weighty = 1;
        final JSlider slidRed = buildSlider(pSelectColor, cc);
        final JSlider slidGreen = buildSlider(pSelectColor, cc);
        final JSlider slidBlue = buildSlider(pSelectColor, cc);
        pSelectColor.add(pColor, cc);
        final JComboBox comboColores = buildColorCombo();
        comboColores.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                Color color = (Color) comboColores.getSelectedItem();
                slidRed.setValue(color.getRed());
                slidGreen.setValue(color.getGreen());
                slidBlue.setValue(color.getBlue());
            }
        });
        comboColores.setSelectedIndex(selectedIndex);
        cc.gridy = 1;
        cc.gridwidth = 4;
        cc.weighty = 0;
        pSelectColor.add(comboColores, cc);
        ChangeListener changeListener = new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                setBackgroundToLabel(pColor, new Color(slidRed.getValue(), slidGreen.getValue(), slidBlue.getValue()));
                calculateMixes();
            }
        };
        slidRed.addChangeListener(changeListener);
        slidGreen.addChangeListener(changeListener);
        slidBlue.addChangeListener(changeListener);
        pSelectColor.setBorder(BorderFactory.createBevelBorder(BevelBorder.LOWERED));
        changeListener.stateChanged(null);
        return pSelectColor;
    }

    private JComboBox buildColorCombo() {
        Color TRANSPARENT = new Color(0, 0, 0, 0);

        Vector<Color> colors = new Vector<Color>();

        colors.add(new NamedColor(Color.RED, "Red"));
        colors.add(new NamedColor(Color.GREEN, "Green"));
        colors.add(new NamedColor(Color.BLUE, "Blue"));

        colors.add(new NamedColor(Color.YELLOW, "Yellow"));
        colors.add(new NamedColor(Color.MAGENTA, "Magenta"));
        colors.add(new NamedColor(Color.CYAN, "Cyan"));

        colors.add(new NamedColor(Color.WHITE, "White"));
        colors.add(new NamedColor(Color.LIGHT_GRAY, "Light Gray"));
        colors.add(new NamedColor(Color.GRAY, "Gray"));
        colors.add(new NamedColor(Color.DARK_GRAY, "Dark Gray"));
        colors.add(new NamedColor(Color.BLACK, "Black"));
        colors.add(new NamedColor(Color.PINK, "Pink"));
        colors.add(new NamedColor(Color.ORANGE, "Orange"));

        colors.add(new NamedColor(TRANSPARENT, "transparent"));
        //http://www.w3schools.com/css/css_colornames.asp
        colors.add(new NamedColor(new Color(0xf0f8ff), "aliceblue"));
        colors.add(new NamedColor(new Color(0xfaebd7), "antiquewhite"));
        colors.add(new NamedColor(new Color(0x00ffff), "aqua"));
        colors.add(new NamedColor(new Color(0x7fffd4), "aquamarine"));
        colors.add(new NamedColor(new Color(0xf0ffff), "azure"));
        colors.add(new NamedColor(new Color(0xf5f5dc), "beige"));
        colors.add(new NamedColor(new Color(0xffe4c4), "bisque"));
        colors.add(new NamedColor(new Color(0x000000), "black"));
        colors.add(new NamedColor(new Color(0xffebcd), "blanchedalmond"));
        colors.add(new NamedColor(new Color(0x0000ff), "blue"));
        colors.add(new NamedColor(new Color(0x8a2be2), "blueviolet"));
        colors.add(new NamedColor(new Color(0xa52a2a), "brown"));
        colors.add(new NamedColor(new Color(0xdeb887), "burlywood"));
        colors.add(new NamedColor(new Color(0x5f9ea0), "cadetblue"));
        colors.add(new NamedColor(new Color(0x7fff00), "chartreuse"));
        colors.add(new NamedColor(new Color(0xd2691e), "chocolate"));
        colors.add(new NamedColor(new Color(0xff7f50), "coral"));
        colors.add(new NamedColor(new Color(0x6495ed), "cornflowerblue"));
        colors.add(new NamedColor(new Color(0xfff8dc), "cornsilk"));
        colors.add(new NamedColor(new Color(0xdc143c), "crimson"));
        colors.add(new NamedColor(new Color(0x00ffff), "cyan"));
        colors.add(new NamedColor(new Color(0x00008b), "darkblue"));
        colors.add(new NamedColor(new Color(0x008b8b), "darkcyan"));
        colors.add(new NamedColor(new Color(0xb8860b), "darkgoldenrod"));
        colors.add(new NamedColor(new Color(0xa9a9a9), "darkgray"));
        colors.add(new NamedColor(new Color(0xa9a9a9), "darkgrey"));
        colors.add(new NamedColor(new Color(0x006400), "darkgreen"));
        colors.add(new NamedColor(new Color(0xbdb76b), "darkkhaki"));
        colors.add(new NamedColor(new Color(0x8b008b), "darkmagenta"));
        colors.add(new NamedColor(new Color(0x556b2f), "darkolivegreen"));
        colors.add(new NamedColor(new Color(0xff8c00), "darkorange"));
        colors.add(new NamedColor(new Color(0x9932cc), "darkorchid"));
        colors.add(new NamedColor(new Color(0x8b0000), "darkred"));
        colors.add(new NamedColor(new Color(0xe9967a), "darksalmon"));
        colors.add(new NamedColor(new Color(0x8fbc8f), "darkseagreen"));
        colors.add(new NamedColor(new Color(0x483d8b), "darkslateblue"));
        colors.add(new NamedColor(new Color(0x2f4f4f), "darkslategray"));
        colors.add(new NamedColor(new Color(0x2f4f4f), "darkslategrey"));
        colors.add(new NamedColor(new Color(0x00ced1), "darkturquoise"));
        colors.add(new NamedColor(new Color(0x9400d3), "darkviolet"));
        colors.add(new NamedColor(new Color(0xff1493), "deeppink"));
        colors.add(new NamedColor(new Color(0x00bfff), "deepskyblue"));
        colors.add(new NamedColor(new Color(0x696969), "dimgray"));
        colors.add(new NamedColor(new Color(0x696969), "dimgrey"));
        colors.add(new NamedColor(new Color(0x1e90ff), "dodgerblue"));
        colors.add(new NamedColor(new Color(0xb22222), "firebrick"));
        colors.add(new NamedColor(new Color(0xfffaf0), "floralwhite"));
        colors.add(new NamedColor(new Color(0x228b22), "forestgreen"));
        colors.add(new NamedColor(new Color(0xff00ff), "fuchsia"));
        colors.add(new NamedColor(new Color(0xdcdcdc), "gainsboro"));
        colors.add(new NamedColor(new Color(0xf8f8ff), "ghostwhite"));
        colors.add(new NamedColor(new Color(0xffd700), "gold"));
        colors.add(new NamedColor(new Color(0xdaa520), "goldenrod"));
        colors.add(new NamedColor(new Color(0x808080), "gray"));
        colors.add(new NamedColor(new Color(0x808080), "grey"));
        colors.add(new NamedColor(new Color(0x008000), "green"));
        colors.add(new NamedColor(new Color(0xadff2f), "greenyellow"));
        colors.add(new NamedColor(new Color(0xf0fff0), "honeydew"));
        colors.add(new NamedColor(new Color(0xff69b4), "hotpink"));
        colors.add(new NamedColor(new Color(0xcd5c5c), "indianred"));
        colors.add(new NamedColor(new Color(0x4b0082), "indigo"));
        colors.add(new NamedColor(new Color(0xfffff0), "ivory"));
        colors.add(new NamedColor(new Color(0xf0e68c), "khaki"));
        colors.add(new NamedColor(new Color(0xe6e6fa), "lavender"));
        colors.add(new NamedColor(new Color(0xfff0f5), "lavenderblush"));
        colors.add(new NamedColor(new Color(0x7cfc00), "lawngreen"));
        colors.add(new NamedColor(new Color(0xfffacd), "lemonchiffon"));
        colors.add(new NamedColor(new Color(0xadd8e6), "lightblue"));
        colors.add(new NamedColor(new Color(0xf08080), "lightcoral"));
        colors.add(new NamedColor(new Color(0xe0ffff), "lightcyan"));
        colors.add(new NamedColor(new Color(0xfafad2), "lightgoldenrodyellow"));
        colors.add(new NamedColor(new Color(0xd3d3d3), "lightgray"));
        colors.add(new NamedColor(new Color(0xd3d3d3), "lightgrey"));
        colors.add(new NamedColor(new Color(0x90ee90), "lightgreen"));
        colors.add(new NamedColor(new Color(0xffb6c1), "lightpink"));
        colors.add(new NamedColor(new Color(0xffa07a), "lightsalmon"));
        colors.add(new NamedColor(new Color(0x20b2aa), "lightseagreen"));
        colors.add(new NamedColor(new Color(0x87cefa), "lightskyblue"));
        colors.add(new NamedColor(new Color(0x778899), "lightslategray"));
        colors.add(new NamedColor(new Color(0x778899), "lightslategrey"));
        colors.add(new NamedColor(new Color(0xb0c4de), "lightsteelblue"));
        colors.add(new NamedColor(new Color(0xffffe0), "lightyellow"));
        colors.add(new NamedColor(new Color(0x00ff00), "lime"));
        colors.add(new NamedColor(new Color(0x32cd32), "limegreen"));
        colors.add(new NamedColor(new Color(0xfaf0e6), "linen"));
        colors.add(new NamedColor(new Color(0xff00ff), "magenta"));
        colors.add(new NamedColor(new Color(0x800000), "maroon"));
        colors.add(new NamedColor(new Color(0x66cdaa), "mediumaquamarine"));
        colors.add(new NamedColor(new Color(0x0000cd), "mediumblue"));
        colors.add(new NamedColor(new Color(0xba55d3), "mediumorchid"));
        colors.add(new NamedColor(new Color(0x9370d8), "mediumpurple"));
        colors.add(new NamedColor(new Color(0x3cb371), "mediumseagreen"));
        colors.add(new NamedColor(new Color(0x7b68ee), "mediumslateblue"));
        colors.add(new NamedColor(new Color(0x00fa9a), "mediumspringgreen"));
        colors.add(new NamedColor(new Color(0x48d1cc), "mediumturquoise"));
        colors.add(new NamedColor(new Color(0xc71585), "mediumvioletred"));
        colors.add(new NamedColor(new Color(0x191970), "midnightblue"));
        colors.add(new NamedColor(new Color(0xf5fffa), "mintcream"));
        colors.add(new NamedColor(new Color(0xffe4e1), "mistyrose"));
        colors.add(new NamedColor(new Color(0xffe4b5), "moccasin"));
        colors.add(new NamedColor(new Color(0xffdead), "navajowhite"));
        colors.add(new NamedColor(new Color(0x000080), "navy"));
        colors.add(new NamedColor(new Color(0xfdf5e6), "oldlace"));
        colors.add(new NamedColor(new Color(0x808000), "olive"));
        colors.add(new NamedColor(new Color(0x6b8e23), "olivedrab"));
        colors.add(new NamedColor(new Color(0xffa500), "orange"));
        colors.add(new NamedColor(new Color(0xff4500), "orangered"));
        colors.add(new NamedColor(new Color(0xda70d6), "orchid"));
        colors.add(new NamedColor(new Color(0xeee8aa), "palegoldenrod"));
        colors.add(new NamedColor(new Color(0x98fb98), "palegreen"));
        colors.add(new NamedColor(new Color(0xafeeee), "paleturquoise"));
        colors.add(new NamedColor(new Color(0xd87093), "palevioletred"));
        colors.add(new NamedColor(new Color(0xffefd5), "papayawhip"));
        colors.add(new NamedColor(new Color(0xffdab9), "peachpuff"));
        colors.add(new NamedColor(new Color(0xcd853f), "peru"));
        colors.add(new NamedColor(new Color(0xffc0cb), "pink"));
        colors.add(new NamedColor(new Color(0xdda0dd), "plum"));
        colors.add(new NamedColor(new Color(0xb0e0e6), "powderblue"));
        colors.add(new NamedColor(new Color(0x800080), "purple"));
        colors.add(new NamedColor(new Color(0xff0000), "red"));
        colors.add(new NamedColor(new Color(0xbc8f8f), "rosybrown"));
        colors.add(new NamedColor(new Color(0x4169e1), "royalblue"));
        colors.add(new NamedColor(new Color(0x8b4513), "saddlebrown"));
        colors.add(new NamedColor(new Color(0xfa8072), "salmon"));
        colors.add(new NamedColor(new Color(0xf4a460), "sandybrown"));
        colors.add(new NamedColor(new Color(0x2e8b57), "seagreen"));
        colors.add(new NamedColor(new Color(0xfff5ee), "seashell"));
        colors.add(new NamedColor(new Color(0xa0522d), "sienna"));
        colors.add(new NamedColor(new Color(0xc0c0c0), "silver"));
        colors.add(new NamedColor(new Color(0x87ceeb), "skyblue"));
        colors.add(new NamedColor(new Color(0x6a5acd), "slateblue"));
        colors.add(new NamedColor(new Color(0x708090), "slategray"));
        colors.add(new NamedColor(new Color(0x708090), "slategrey"));
        colors.add(new NamedColor(new Color(0xfffafa), "snow"));
        colors.add(new NamedColor(new Color(0x00ff7f), "springgreen"));
        colors.add(new NamedColor(new Color(0x4682b4), "steelblue"));
        colors.add(new NamedColor(new Color(0xd2b48c), "tan"));
        colors.add(new NamedColor(new Color(0x008080), "teal"));
        colors.add(new NamedColor(new Color(0xd8bfd8), "thistle"));
        colors.add(new NamedColor(new Color(0xff6347), "tomato"));
        colors.add(new NamedColor(new Color(0x40e0d0), "turquoise"));
        colors.add(new NamedColor(new Color(0xee82ee), "violet"));
        colors.add(new NamedColor(new Color(0xf5deb3), "wheat"));
        colors.add(new NamedColor(new Color(0xffffff), "white"));
        colors.add(new NamedColor(new Color(0xf5f5f5), "whitesmoke"));
        colors.add(new NamedColor(new Color(0xffff00), "yellow"));
        colors.add(new NamedColor(new Color(0x9acd32), "yellowgreen"));

        JComboBox comboBox = new JComboBox(new DefaultComboBoxModel(colors));
        comboBox.setRenderer(new DefaultListCellRenderer() {
            protected Color backgroundColor = Color.BLACK;

            {
                setBorder(new CompoundBorder(
                        new MatteBorder(2, 5, 2, 5, Color.white)
                        , new LineBorder(Color.black)));
            }

            public Component getListCellRendererComponent(JList list, Object obj,
                                                          int row, boolean sel, boolean hasFocus) {
                if (obj instanceof Color)
                    backgroundColor = (Color) obj;
                setText(obj.toString());
                return this;
            }

            public void paint(Graphics g) {
                setBackground(backgroundColor);
                super.paint(g);
            }
        });


        return comboBox;
    }

    class NamedColor extends Color {
        private String name;

        NamedColor(Color color, String name) {
            super(color.getRed(), color.getGreen(), color.getBlue());
            this.name = name;
        }

        @Override
        public String toString() {
            return name;
        }
    }

    private void calculateMixes() {
        calculateFirstMix();
        calculateSecondMix();
    }

    private void calculateFirstMix() {
        calculateMix(firstMixColors, firstMixColor);
    }

    private void calculateSecondMix() {
        calculateMix(secondMixColors, finalColor);
    }

    private void calculateMix(Vector<JLabel> mixColors, JLabel finalColor) {
        Color bg = ((Mixer) comboBox.getSelectedItem()).calculateMix(mixColors);
        setBackgroundToLabel(finalColor, bg);
    }

    private void setBackgroundToLabel(JLabel label, Color color) {
        label.setBackground(color);
        label.setText(color.getRed() + "," + color.getGreen() + "," + color.getBlue());
    }

    interface Mixer {
        Color calculateMix(Vector<JLabel> colores);
    }

    /**
     * Implement a additive mix of colors
     */
    static class AdditiveMixer implements Mixer {
        public Color calculateMix(Vector<JLabel> colores) {
            int red = 0;
            int green = 0;
            int blue = 0;
            for (int i = 0; i < colores.size(); i++) {
                Color background = colores.get(i).getBackground();
                red += background.getRed();
                green += background.getGreen();
                blue += background.getBlue();
            }
            return new Color(Math.min(255, red), Math.min(255, green), Math.min(255, blue));
        }

        @Override
        public String toString() {
            return "Additive";
        }
    }

    /**
     * Implement a sustractive mix of colors
     */
    static class SustractiveMixer implements Mixer {
        public Color calculateMix(Vector<JLabel> colores) {
            int red = 1;
            int green = 1;
            int blue = 1;
            for (int i = 0; i < colores.size(); i++) {
                Color background = colores.get(i).getBackground();
                red *= background.getRed();
                green *= background.getGreen();
                blue *= background.getBlue();
            }
            return new Color(Math.min(255, red / 255), Math.min(255, green / 255), Math.min(255, blue / 255));
        }

        @Override
        public String toString() {
            return "Sustractive";
        }
    }

    /**
     * Implement a diluting/sustractive mix of colors
     */
    static class DilutingSustractiveMixer implements Mixer {
        public Color calculateMix(Vector<JLabel> colores) {
            int red = 0;
            int green = 0;
            int blue = 0;
            for (int i = 0; i < colores.size(); i++) {
                Color background = colores.get(i).getBackground();
                red += Math.pow(255 - background.getRed(), 2);
                green += Math.pow(255 - background.getGreen(), 2);
                blue += Math.pow(255 - background.getBlue(), 2);
            }
            return new Color(Math.min(255, (int)Math.sqrt(red / colores.size())), Math.min(255, (int)Math.sqrt(green / colores.size())), Math.min(255, (int)Math.sqrt(blue / colores.size())));
        }

        @Override
        public String toString() {
            return "Diluting/Sustractive";
        }
    }

    /**
     * Implement a diluting/sustractive mix of colors
     */
    static class TertiaryMixer implements Mixer {
        public Color calculateMix(Vector<JLabel> colores) {
            Color background1 = colores.get(0).getBackground();
            int red = background1.getRed();
            int green = background1.getGreen();
            int blue = background1.getBlue();
            Color background2 = colores.get(1).getBackground();
            red -= background2.getRed();
            green -= background2.getGreen();
            blue -= background2.getBlue();
            return new Color(Math.min(255, background1.getRed() - (red/2)), Math.min(255, background1.getGreen() - (green/2)), background1.getBlue() - (blue/2));
        }

        @Override
        public String toString() {
            return "Tertiary";
        }
    }

    private JSlider buildSlider(JPanel container, GridBagConstraints upperCC) {
        JPanel panel = new JPanel(new GridBagLayout());
        GridBagConstraints cc = new GridBagConstraints();
        cc.fill = GridBagConstraints.BOTH;
        cc.insets = new Insets(5, 5, 5, 5);
        cc.weightx = 1;
        cc.weighty = 0.7;

        final JSlider slider = new JSlider(JSlider.VERTICAL, 0, 255, 0);
        slider.setFont(new Font("Serif", Font.PLAIN, 4));

        Hashtable<Integer, JLabel> labels = new Hashtable<Integer, JLabel>();
        labels.put(0, new JLabel("0"));
        labels.put(128, new JLabel("128"));
        labels.put(255, new JLabel("255"));
        panel.add(slider, cc);
        final JTextField field = new JTextField();
        field.setEditable(false);
        slider.addChangeListener(new ChangeListener() {
            @Override
            public void stateChanged(ChangeEvent e) {
                field.setText(String.valueOf(slider.getValue()));
            }
        });
        cc.gridx = 0;
        cc.gridy = 1;
        cc.weighty = 0;

        panel.add(field, cc);
        slider.setLabelTable(labels);
        slider.setPaintLabels(true);

        container.add(panel, upperCC);

        return slider;
    }
}
