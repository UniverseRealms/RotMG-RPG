package kabam.rotmg.ui.view {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.Player;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;

import kabam.rotmg.messaging.impl.outgoing.IncrementStat;

import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.ui.signals.UpdateHUDSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class StatMetersMediator extends Mediator {

    [Inject]
    public var view:StatMetersView;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var updateHUD:UpdateHUDSignal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;


    override public function initialize():void {
        this.updateHUD.add(this.onUpdateHUD);
    }

    override public function destroy():void {
        this.updateHUD.add(this.onUpdateHUD);
    }

    private function onUpdateHUD(_arg1:Player):void {
        this.view.update(_arg1);
    }

    /*private function onIncrement(_arg1:String):void {
        this.view.statIncrement.x -= 10;
        switch(_arg1)
        {
            case "Hp":
                IncStat(0);
                break;
            default:
                this.view.statIncrement.x += 10;
                break;
        }
    }

    private function IncStat(_arg1:int){
        var _local1:IncrementStat = (this.messages.require(GameServerConnection.STATINCREMENT) as kabam.rotmg.messaging.impl.outgoing.IncrementStat);
        _local1.statType  = _arg1;
        this.socketServer.queueMessage(_local1);
    }*/
}
}
