   M S B A M L   `   `   `  ����C  ?Spectrum, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:SpectrumSpectrum  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       Spectrum.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   +spectrumclr-namespace:Spectrum     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$  Spectrum2RGB��5   	   $	��600��6   $
��1060��6+    � 	Resources 5         ColourSpaces S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName��%�   &	        S� 5   
   $ 	GetValues��62   "  5	       5
      M� 5      $ spectrum:ColourSpaceType��6   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $	��860 �6   6-   �� 5      $��* �6   6+   .+�5   
   ]� $��0��6   .+�5      �� $	��840��6   $	��540��6!   
���   �$��1q�6B   ._�5         Spectrum.SpectrumPlot  -   5          Name$ SpectrumPlot��5   (   6@   5      5      � 5   
   $��1��6    � RowDefinitions 5      �� 5      $
��Auto �6    60   �� 5      6"   .+�5      �� -   5        	 ��Name$	 ColourSpaceComboBox��5      $��0��64   $��40��5       
 �� SelectedIndex$
 0��6#   $��20(�65   ��5   A   ��  �� Source# [  5"      � 5#      $��1��6    5$      �� 5%      $
��Auto �6$   64   �� 5&      $
��Auto �6$   64   �� 5'      $
��Auto �6$   64   .+�5)      �� $��1��6   $��40��6)   $��Selected color��65    �� TextAlignment$ Center=�6K   $��25(�6b   6r   �� 5*      -   5         ��Name$ SelectedColorCanvas��5*      $��2��66   $	��100��6C   $	��100��6P   6^   �� 5+      $��0��6    �� Orientation$ 
Horizontal=�6*   .+�5,      �� -   5         ��Name$ ConvertButton��5,      $��40��64   $	��100��6@   $��20(�6L   .��6v   	Convert6   �� 5-      -   5        $ ResetButton��5-      $��40��62   $��90��6>   $��20(�6I   .��6q   Clear6x   5.      5/      50      51      52      