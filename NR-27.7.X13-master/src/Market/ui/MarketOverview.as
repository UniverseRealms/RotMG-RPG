package Market.ui
{

import com.company.assembleegameclient.account.ui.Frame;

import com.company.assembleegameclient.screens.TitleMenuOption;

import com.company.assembleegameclient.ui.DeprecatedClickableText;
import com.company.assembleegameclient.util.Currency;
import com.company.util.MoreColorUtil;

import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.ColorMatrixFilter;
import flash.filters.DropShadowFilter;

import kabam.rotmg.account.core.view.EmptyFrame;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.util.components.LegacyBuyButton;

import mx.states.AddChild;
import mx.states.RemoveChild;

import org.osflash.signals.IOnceSignal;
import org.osflash.signals.Signal;
import com.company.assembleegameclient.ui.icons.IconButton;

import flash.display.Sprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton
   import flash.text.TextFieldAutoSize;
   import flashx.textLayout.formats.VerticalAlign;


import kabam.rotmg.pets.view.components.DialogCloseButton;

import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

public class MarketOverview extends EmptyFrame
   {


      private var currentScreen:Sprite;
      
      private var myOffersButton:TitleMenuOption;
      
      private var createOfferButton:TitleMenuOption;

      public function MarketOverview()
      {
         super();
         var _loc1_:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(46).setBold(true).setStringBuilder(new StaticStringBuilder("Market")).setAutoSize(TextFieldAutoSize.CENTER).setTextWidth(600).setColor(16777215).setPosition(0,20);
         AddChild(_loc1_);
         AddChild(this.closeButton);
         this.myOffersButton = this.createButton("My offers",10,this.myOffers,true);
         this.createOfferButton = this.createButton("Create offer",110,this.createOffer);
         this.myOffers();
      }

      private function createOffer() : void
      {
         var screen:MarketCreateOfferScreen = new MarketCreateOfferScreen();
         screen.removed.add(function():void
         {
            createOfferButton.activate();
         });
         this.setScreen(screen);
         this.createOfferButton.deactivate();
      }

      public function myOffers() : void
      {
         var screen:MarketMyOffersScreen = new MarketMyOffersScreen();
         screen.removed.add(function():void
         {
            myOffersButton.activate();
         });
         this.setScreen(screen);
         this.myOffersButton.deactivate();
      }

      public function setScreen(param1:Sprite) : void
      {
         if(this.currentScreen && contains(this.currentScreen))
         {
            RemoveChild(this.currentScreen);
         }
         this.currentScreen = param1;
         this.currentScreen.y = 100;
         AddChild(param1);
      }
      
      override protected function makeModalBackground() : Sprite
      {
         x = 0;
         y = 0;
         var _loc1_:Sprite = new Sprite();
         _loc1_.graphics.beginFill(0,0.9);
         _loc1_.graphics.drawRect(0,0,600,600);
         _loc1_.graphics.endFill();
         _loc1_.graphics.beginFill(16777215,1);
         _loc1_.graphics.drawRect(0,98,600,2);
         _loc1_.graphics.endFill();
         return _loc1_;
      }

      private function createButton(param1:String, param2:Number, param3:Function, param4:Boolean = false) : TitleMenuOption
      {
         var _loc5_:TitleMenuOption = null;
         _loc5_ = new TitleMenuOption(param1,18,false);
         _loc5_.setAutoSize(TextFieldAutoSize.LEFT);
         _loc5_.setVerticalAlign(VerticalAlign.MIDDLE);
         _loc5_.x = param2;
         _loc5_.y = 85;
         _loc5_.clicked.add(param3);
         if(param4)
         {
            _loc5_.deactivate();
         }
         AddChild(_loc5_);
         return _loc5_;
      }
   }
}
