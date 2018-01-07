package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.Player;

import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.IncrementStat;

import kabam.rotmg.ui.signals.UpdateHUDSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class StatMediator extends Mediator {

    [Inject]
    public var view:StatView;
    [Inject]
    public var updateHUD:UpdateHUDSignal;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;

    override public function initialize():void {
        view.incSignal.add(OnIncrement);
    }

    override public function destroy():void {
    }

    private function OnIncrement(_arg1:int):void {
        IncStat(_arg1);
    }

    private function IncStat(_arg1:int):void{
        var _local1:IncrementStat = (this.messages.require(GameServerConnection.STATINCREMENT) as IncrementStat);
        _local1.statType  = _arg1;
        this.socketServer.queueMessage(_local1);
    }

    private function onMouseOver(_arg1:MouseEvent):void {
    }

    private function onMouseOut(_arg1:MouseEvent):void {
    }
}
}
