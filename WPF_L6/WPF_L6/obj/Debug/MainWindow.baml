   M S B A M L   `   `   `  ����A  =WPF_L6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPF_L6WPF_L6  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       WPF_L6.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:WPF_L6     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   X TXceed.Wpf.Toolkit, Version=4.7.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  
MainWindow��5
   	   $	��750��6   $	��650��6)   .��5      � .+�5   
   �� -   5         ��Name$ Country��5      $
��Left=�6$   $��
160,62,0,0q�6?   $	��Top=�6S   $	��120��6k   .@�5      ��Россия��6!   ��5      Финляндия��6$   ��5      Григория��6#   ��5      Франция��6"   5      �� 5   
   $
��Left=�6   $��
285,10,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   $��24(�6o   �� 6~    � Language$ ru-ru�6�   $�ЗАЯВОЧКА��6�   6�   �� 5   
   $
��Left=�6   $��	79,60,0,0q�6/   $
��Wrap=�6B   $	��Top=�6V   $��18(�6n   �� 6}   $ ru-ru�6�   $�Страна:��6�   6�   �� 5   
   $
��Left=�6   $��
79,124,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   $��18(�6o   �� 6~   $ ru-ru�6�   $�Телефон:��6�   6�   �� 5   
   -   5         ��Name$ 	PhoneType��5      $
��Left=�6%   $��55��6@   $��166,106,0,0q�6L   $	��Top=�6a   $	��108��6y    �� SelectionMode$ Single=�6�   $��16(�6�   .@�5      ��Мобильный��6$   ��5      Домашний��6#   5      �� 5   
   -   5         ��Name$ 	PhoneCode��5      $
��Left=�6'   $��356,124,0,0q�6B   $
��Wrap=�6W   $	��Top=�6k   $��18(�6�   $��+7��6�   %  Xceed.Wpf.Toolkit.MaskedTextBox  5   
   -   5          Name$ PhoneNumber��5      $
��Left=�62   $��27��6M   $��392,124,0,0q�6Y    �� TextWrapping$
 Wrap=�6n   $	��Top=�6�   $	��162��6�   $��18(�6�   �� 5   
   $
��Left=�6   $��
79,272,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   $��18(�6o   �� 6~   $�Фамилия��6�   �� 6�   $ ru-ru�6�   $�:��6�   6�   �� 5   
   -   5         ��Name$ Surname��5      $
��Left=�6#   $��170,272,0,0q�6>   $
 Wrap=�6S   $	��Top=�6g   $��18(�6   $	��172��6�   ��6�   �� *	Surname+	 �� ValidatesOnDataErrors	 . 
 �� UpdateSourceTrigger$
 PropertyChanged=��� 5   
   $
��Left=�6   $��366,272,0,0q�6/   $
��Wrap=�6D   $	��Top=�6X   $��18(�6p   �� 6   $ ru-ru�6�   $�Имя:��6�   6�   �� 5   
   -   5        $ 	Firstname��5      $
��Left=�6%   $��410,272,0,0q�6@   $
 Wrap=�6U   $	��Top=�6i   $��18(�6�   $	��170��6�   ��6�   �� *	Firstname+	 . $
 PropertyChanged=��� 5   
   $
��Left=�6   $��
79,329,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   $��18(�6o   �� 6~   $ ru-ru�6�   $ �Дата рождения:��6�   6�     5    
   -   5        $	 day��5       $
��Left=�6*   $��222,330,0,0q�6E      Mask$ 00��6Z   $
 Wrap=�6d   $	��Top=�6x   $��22��6�   $��16(�6�   6�   �� 5!   
   -   5        $ MonthsComboBox��5!      $
��Left=�6+   $��256,330,0,0q�6F   $	��Top=�6[   $	��122��6s   6     5"   
   -	   5        $
 year��5"      $
��Left=�6+   $��22��6F   $��394,330,0,0q�6R   $
 Wrap=�6g   $
 0000��6{   $	��Top=�6�   $��44��6�   $��16(�6�   6�   �� 5#   
   $
��Left=�6   $��
79,412,0,0q�6/   $
��Wrap=�6C   $	��Top=�6W   $��14(�6o    �� TextAlignment$ Center=�6}   �� 6�   $ ru-ru�6�   $���Внимание! Проверьте введенные Вами данные, прежде чем отправить заявку!��6�   6�   �� 5$   
   -
   5         ��Name$ Sender��5$      $��ОТПРАВИТЬ��6!   $
��Left=�65   $��32��6P   $��201,486,0,0q�6\   $	��Top=�6q   $	��237��6�   6�   �� 5%   
   -   5        $ Err_Surname��5%      $
��Left=�6)   $��170,303,0,0q�6D   $
��Wrap=�6Y   
���  ��$	��Top=�6�   $��8(�6�   $	��172��6�   ��6m   �� *[Surname].ErrorContent+6�   �� 5&   
   -   5        $ Err_Firstname��5&      $
��Left=�6+   $��410,303,0,0q�6F   $
��Wrap=�6[   
���  ��$	��Top=�6�   $��8(�6�   $	��170��6�   ��6o   �� *[Firstname].ErrorContent+6�   5'      5(      