package Market.ui
{
import com.company.assembleegameclient.screens.TitleMenuOption;
import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.account.core.view.EmptyFrame;
import kabam.rotmg.messaging.impl.incoming.Text;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import flash.text.TextFieldAutoSize;
import flashx.textLayout.formats.VerticalAlign;

import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

public class MarketOverview extends EmptyFrame
{
    private var currentScreen:Sprite;
    private var myOffersButton:TitleMenuOption;
    private var createOfferButton:TitleMenuOption;

    private var closeButtonTxt:TextFieldDisplayConcrete;

    public function MarketOverview()
    {
        super();
        var _local1:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(46).setBold(true).setStringBuilder(new StaticStringBuilder("Market")).
        setAutoSize(TextFieldAutoSize.CENTER).setTextWidth(600).setColor(16777215).setPosition(0,20);
        addChild(_local1);
        this.myOffersButton = this.createButton("My offers",10,this.myOffers,true);
        this.createOfferButton = this.createButton("Create offer",110,this.createOffer);
        this.myOffers();
        this.makeCloseButton();
    }

    private function makeCloseButton():void {
        this.closeButtonTxt = new TextFieldDisplayConcrete().setSize(16).setColor(0xFFFFFF).setBold(true);
        this.closeButtonTxt.setStringBuilder(new StaticStringBuilder("CLOSE"));
        this.closeButtonTxt.setPosition(240, 60);

        this.closeButtonTxt.addEventListener(MouseEvent.CLICK, onClose);

        addChild(this.closeButtonTxt);
    }

    private function onClose(_arg1:MouseEvent):void {
        this.parent.removeChild(this);
    }

    private function createOffer() : void
    {
        var _local1:MarketCreateOfferScreen = new MarketCreateOfferScreen();
        _local1.removed.add(function():void
        {
            createOfferButton.activate();
        });
        this.setScreen(_local1);
        this.createOfferButton.deactivate();
    }

    private function createButton(param1:String, param2:Number, param3:Function, param4:Boolean = false) : TitleMenuOption
    {
        var _local1:TitleMenuOption = null;
        _local1 = new TitleMenuOption(param1,18,false);
        _local1.setAutoSize(TextFieldAutoSize.LEFT);
        _local1.setVerticalAlign(VerticalAlign.MIDDLE);
        _local1.x = param2;
        _local1.y = 85;
        _local1.clicked.add(param3);
        if(param4)
        {
            _local1.deactivate();
        }
        addChild(_local1);
        return _local1;
    }

    protected override function makeModalBackground():Sprite {
        x = 0;
        y = 0;
        var _local1:Sprite = new Sprite();
        _local1.graphics.beginFill(0,0.9);
        _local1.graphics.drawRect(0,0,600,600);
        _local1.graphics.endFill();
        _local1.graphics.beginFill(16777215,1);
        _local1.graphics.drawRect(0,98,600,2);
        _local1.graphics.endFill();
        return(_local1);
    }

    public function setScreen(param1:Sprite) : void
    {
        if(this.currentScreen && contains(this.currentScreen))
        {
            removeChild(this.currentScreen);
        }
        this.currentScreen = param1;
        this.currentScreen.y = 100;
        addChild(param1);
    }

    public function myOffers() : void
    {
        var _local1:MarketMyOffersScreen = new MarketMyOffersScreen();
        _local1.removed.add(function():void
        {
            myOffersButton.activate();
        });
        this.setScreen(_local1);
        this.myOffersButton.deactivate();
    }
}
}
