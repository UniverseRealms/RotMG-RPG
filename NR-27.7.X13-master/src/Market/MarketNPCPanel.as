package Market
{
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.panels.Panel

import flash.display.Bitmap;
import flash.events.MouseEvent;

import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

import com.company.assembleegameclient.game.GameSprite;

import org.osflash.signals.Signal;

public class MarketNPCPanel extends Panel
   {


      private const titleText:TextFieldDisplayConcrete = PetsViewAssetFactory.returnTextfield(16777215,16,true);

      private var icon:Bitmap;

     internal var _0R_w:DeprecatedTextButton;

      internal var _0J___:DeprecatedTextButton;

      private var _title:String = "Marketplace";

      private var _sell:String = "Info";

      private var _manage:String = "Manage";

      private var type:uint;

       public var onManageSignal:Signal = new Signal();

      public function MarketNPCPanel(param1:GameSprite, param2:uint)
      {
         this.type = param2;
         super(param1);
      }

      private function _0Y_I_() : void
      {
          this._0R_w = new DeprecatedTextButton(16,this._sell,0,true);
          this._0R_w.textChanged.addOnce(this._17U_);
          addChild(this._0R_w);
      }

      private function _A_v() : void
      {
         this.icon.x = 16;
         this.icon.y = -2;
         this.titleText.setStringBuilder(new LineBuilder().setParams(this._title));
         this.titleText.x = 69;
         this.titleText.y = 28;
         addChild(this.titleText);
      }

      private function _1Q_D_() : void
      {
         this._0J___ = new DeprecatedTextButton(16,this._manage,0,true);
          this._0J___.addEventListener(MouseEvent.CLICK, onManage);
         this._0J___.textChanged.addOnce(this._17U_);
         addChild(this._0J___);
      }

       private function onManage(_arg1:MouseEvent):void {
           this.onManageSignal.dispatch();
       }

      public function init() : void
      {
         this._1e8();
         this._A_v();
         this._0Y_I_();
         this._1Q_D_();
      }

      private function _1e8() : void
      {
         this.icon = PetsViewAssetFactory.returnBitmap(this.type);
         this.icon.scaleX = 0.5;
         this.icon.scaleY = 0.5;
         addChild(this.icon);
         this.icon.x = 5;
      }

      private function _17U_() : void
      {
         if(this._0J___ && contains(this._0J___))
         {
            this._0J___.x = WIDTH / 4 * 3 - this._0J___.width / 2;
            this._0J___.y = HEIGHT - this._0J___.height - 4;
            this._0R_w.x = WIDTH / 4 * 1 - this._0R_w.width / 2;
            this._0R_w.y = HEIGHT - this._0R_w.height - 4;
         }
         else
         {
            this._0R_w.x = (WIDTH - this._0R_w.width) / 2;
            this._0R_w.y = HEIGHT - this._0R_w.height - 4;
         }
      }
   }
}
