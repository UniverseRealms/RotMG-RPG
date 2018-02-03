package Market
{


import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import com.company.assembleegameclient.ui.DeprecatedTextButton;

import flash.display.DisplayObjectContainer;
import flash.display.Sprite;


import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.pets.view.components.CaretakerQueryDialogCategoryList;
import kabam.rotmg.pets.view.components.PopupWindowBackground;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.ui.view.SignalWaiter;
import kabam.rotmg.util.graphics.ButtonLayoutHelper;


   import org.osflash.signals.Signal
   import flash.events.MouseEvent;
   import flash.display.BitmapData;
   import org.osflash.signals.natives.NativeMappedSignal;
   import flash.text.TextFieldAutoSize;

public class InfoDialog extends Sprite
   {

      public static const WIDTH:int = 274;

      public static const HEIGHT:int = 428;

      public static const TITLE:String = "Market Info";

      public static const CLOSE:String = "Close";

      public static const BACK:String = "Back";

      public static const Questions:Array = [{
         "category":"marketInfo.about",
         "info":"marketInfo.about.desc"
      },{
         "category":"marketInfo.selling",
         "info":"marketInfo.selling.desc"
      },{
         "category":"marketInfo.buying",
         "info":"marketInfo.buying.desc"
      },{
         "category":"marketInfo.forgot",
         "info":"marketInfo.forgot.desc"
      },{
         "category":"marketInfo.undo",
         "info":"marketInfo.undo.desc"
      },{
         "category":"marketInfo.gimmeBack",
         "info":"marketInfo.gimmeBack.desc"
      }];


      private const _1g3:SignalWaiter = getSignal();

      private const container:DisplayObjectContainer = makeContainer();

      private const background:PopupWindowBackground = getBackGround();

      private const query:InfoDialogQuery = getInfoDialog();

      private const title:TextFieldDisplayConcrete = getTextTitle();

      private const _16P_:CaretakerQueryDialogCategoryList = getCaretaker();

      private const BackBtn:DeprecatedTextButton = CreateBackButton();

      private const CloseBtn:DeprecatedTextButton = CreateCloseButton();

      public function InfoDialog()
      {
         super();
      }

      private function getSignal() : SignalWaiter
      {
         var _loc1_:SignalWaiter = new SignalWaiter();
         _loc1_.complete.addOnce(this.buttonLayout);
         return _loc1_;
      }

      private function buttonLayout() : void
      {
         var _loc1_:ButtonLayoutHelper = new ButtonLayoutHelper();
         _loc1_.layout(WIDTH,this.CloseBtn);
          this.CloseBtn.addEventListener(MouseEvent.CLICK, onClose);
         _loc1_.layout(WIDTH,this.BackBtn);
          this.BackBtn.addEventListener(MouseEvent.CLICK, onBack);
      }

      private function onBack(_arg1:MouseEvent):void {
          this.query.visible = false;
          this._16P_.visible = true;
          this.CloseBtn.visible = true;
      }

      private function onClose(_arg1:MouseEvent):void {
          this.parent.removeChild(this);
      }

      private function makeContainer() : DisplayObjectContainer
      {
         var _loc1_:Sprite = null;
         _loc1_ = new Sprite();
         _loc1_.x = (800 - WIDTH) / 2;
         _loc1_.y = (600 - HEIGHT) / 2;
         addChild(_loc1_);
         return _loc1_;
      }

      private function getBackGround() : PopupWindowBackground
      {
         var _loc1_:PopupWindowBackground = new PopupWindowBackground();
         _loc1_.draw(WIDTH,HEIGHT);
         _loc1_.divide(PopupWindowBackground.HORIZONTAL_DIVISION,34);
         this.container.addChild(_loc1_);
         return _loc1_;
      }

      private function getInfoDialog() : InfoDialogQuery
      {
         var _loc1_:InfoDialogQuery = null;
         _loc1_ = new InfoDialogQuery();
         _loc1_.x = 20;
         _loc1_.y = 50;
         this.container.addChild(_loc1_);
         return _loc1_;
      }

      private function getTextTitle() : TextFieldDisplayConcrete
      {
         var _loc1_:TextFieldDisplayConcrete = null;
        _loc1_ = PetsViewAssetFactory.returnTextfield(0xFFFFFF, 18, true);
         _loc1_.setStringBuilder(new StaticStringBuilder(TITLE));
         _loc1_.setAutoSize(TextFieldAutoSize.CENTER);
         _loc1_.x = WIDTH / 2;
         _loc1_.y = 24;
         this.container.addChild(_loc1_);
         return _loc1_;
      }

      private function CreateBackButton() : DeprecatedTextButton
      {
         var _loc1_:DeprecatedTextButton = new DeprecatedTextButton(16, BACK, 80);
         _loc1_.y = 382;
         _loc1_.visible = false;
         _loc1_.addEventListener(MouseEvent.CLICK,this.BackEvent);
         this.container.addChild(_loc1_);
         this._1g3.push(_loc1_.textChanged);
         return _loc1_;
      }

      private function BackEvent(param1:MouseEvent) : void
      {
         this.query._14E_();
         this._16P_.visible = true;
         this.CloseBtn.visible = true;
         this.BackBtn.visible = false;
      }

      private function CreateCloseButton() : DeprecatedTextButton
      {
         var _loc1_:DeprecatedTextButton = null;
         _loc1_ = new DeprecatedTextButton(16,CLOSE,110,true);
         _loc1_.y = 382;
         this.container.addChild(_loc1_);
         this._1g3.push(_loc1_.textChanged);
         return _loc1_;
      }

      private function getCaretaker() : CaretakerQueryDialogCategoryList {
         var _local1:CaretakerQueryDialogCategoryList = new CaretakerQueryDialogCategoryList(Questions);
         _local1.x = 20;
         _local1.y = 110;
         _local1.selected.add(this.setQuery);
         this.container.addChild(_local1);
         this._1g3.push(_local1.ready);
         return (_local1);
      }

      private function setQuery(param1:String) : void
      {
          this._16P_.visible = false;
          this.CloseBtn.visible = false;
          this.BackBtn.visible = true;
          this.query.visible = true;
          this.query._070(param1);
      }

      public function _0K_L_(param1:BitmapData) : void
      {
          this.query._0K_L_(param1);
      }
   }
}
