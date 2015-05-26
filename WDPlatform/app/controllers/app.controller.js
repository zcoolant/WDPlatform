(function (angular) {
    'use strict';

    /**
	 * AppController
	 * The App Controller is responsible for exposing the number of online users to the view.
	 * 
	 * The controller will invoke the 'getUserCount' on initialization.
	 * Finally, the controller will listen for the 'sendUserCount' 
	 *    event from the hub and update the user count sent by the hub.
	 * 
	 * @param {AppHub}
	 */
    AppController.$inject = ['AppHub', '$scope', '$mdToast'];
    function AppController(AppHub, $scope, $mdToast, $location) {

        // Internal reference
        var self = this;

        // user count property we'll expose to the view
        this.usersOnline = 0;

        this.roomNumber;
        this.userName;
        this.isCreater = false;
        this.isPlayer = false;
        this.isQuestioner = false;
        this.isStart = false;
        this.players = [];
        this.cards = [];
        this.question = "";
        this.status = "WD Platform Beta"
        this.selectCards = [];
        this.allSelectCards = [];
        this.questionerSelected = {};
        this.submitedNumber = 0;
        this.submitDisable = false;
        this.questioner = "";
        this.roundWinner = "";
        this.roundWinnerCards = [];
        this.roundWinnerQuestion = {};

        // Connection status property we'll expose to the view
        this.connectionStatus = AppHub.connectionStatus;

        // Internal hub event handler, simply gets the count from the hub 
        ///    and updates the usersOnline property of the controller
        //
        function updateCount(count) {
            self.usersOnline = count;
        }

        //create new room
        this.create = function () {
            AppHub.invoke('createRoom', this.userName).then(function (roomNumber) {
                console.log('Room Create successfully');
                self.roomNumber = roomNumber;
                self.isCreater = true;
                self.status = "Room-" + self.roomNumber + ": Waiting for players"
            })
        };

        //join a rrom
        this.join = function () {
            AppHub.invoke('joinRoom', this.roomNumber, this.userName).then(function (roomNumber) {
                console.log('Room join successfully');
                self.isCreater = false;
                self.isPlayer = true;
                self.status = "Room-" + self.roomNumber + ": Waiting for players"
            })
        };

        this.start = function () {
            AppHub.invoke('startGame', this.roomNumber).then(function () {
                console.log('Game Start successfully');
            })
        };

        this.submitCards = function () {
            if (self.isQuestioner) {
                self.allSelectCards = [];
                var selected = {};
                for (var k in self.questionerSelected) {
                    if (self.questionerSelected[k]) {
                        selected = k;
                        break;
                    }
                }
                self.questionerSelected = {};
                console.log(selected);
                AppHub.invoke('questionerConfirm', this.roomNumber, this.userName, selected).then(function (fromUser) {
                    console.log('questionerConfirm Cards successfully');
                    self.isQuestioner = false;
                })
            } else {
                var newCards = [];
                for (var i = 0 ; i < self.cards.length; i++) {
                    var cardInMyPack = self.cards[i];
                    if (cardInMyPack.pick > 0) {
                        self.cards.splice(i, 1);
                        self.cards.push(cardInMyPack);
                    }
                }
                AppHub.invoke('submitCards', this.roomNumber, this.userName, this.selectCards).then(function () {
                    console.log('Submit Cards successfully');
                    self.status = "Room-" + self.roomNumber + ": Waiting for " + self.questioner + "'s choice";
                    self.submitedNumber++;
                    self.cards.splice(self.cards.length - self.selectCards.length, self.selectCards.length);
                    self.selectCards.splice(0, self.selectCards.length);
                    if (self.submitedNumber == 1) {
                        self._showMessage("Submit successfully. You can submit 1 more to bid");
                    } else {
                        self._showMessage("Submit successfully.");
                    }
                    
                })
            }
        };

        this.toogleSelected = function (item) {
            console.log(item);
            var idx = self.selectCards.indexOf(item);
            if (idx > -1) {
                self.selectCards.splice(idx, 1);
                item.pick = 0;
            }
            else {
                self.selectCards.push(item);
            }

            for (var i = 0 ; i < self.selectCards.length; i++) {
                var cardInMyPack = self.selectCards[i];
                cardInMyPack.pick = i + 1;
            }
        };

        this.toogleQuestionerSelected = function (item) {
            console.log(item);
            for (var k in self.questionerSelected) {
                if (item[0].text != k) {
                    self.questionerSelected[k] = false;
                }
            }
            if (self.questionerSelected[item[0].text]) {
                self.submitDisable = true;
            } else {
                self.submitDisable = false;
            }

        };

        this._showMessage = function (message) {
            $mdToast.show(
                $mdToast.simple()
                    .content(message)
                    .hideDelay(3000)
                );
        }

        AppHub.on('refreshPlayers', function (players) {
            console.log(players);
            self.players = players;
        });

        AppHub.on('reloadCards', function (cards, question, scores, questioner) {
            console.log(cards, question, scores, questioner);
            //For all
            self.isStart = true;
            self.status = "Room-" + self.roomNumber + ": Select for " + questioner;
            question.text = questioner + ": " + question.text;
            self.question = question;
            self.players = scores;

            //For creater
            self.allSelectCards = [];

            //For player
            for (var i = 0 ; i < cards.length; i++) {
                self.cards.unshift(cards[i]);
            }
            self.questioner = questioner;
            if (self.userName == self.questioner) {
                self.isQuestioner = true;
                self.status = "Room-" + self.roomNumber + ": Waiting for others";
            }
            else self.isQuestioner = false;
            self.submitedNumber = 0;
            self.submitDisable = true;
        });

        AppHub.on('addSelected', function (newAddedSelected) {
            console.log(newAddedSelected);
            self.status = "Room-" + self.roomNumber + ": You can choose now";
            self.allSelectCards.push(newAddedSelected);
        });

        AppHub.on('showRoundWinner', function (winner, selected,winnerQuestion) {
            console.log("Winner and seleted", winner, selected);
            self.roundWinner = winner;
            self.roundWinnerCards = selected;
            self.roundWinnerQuestion = winnerQuestion;
            if (!self.isCreater) {
                $('#showWinnerModal').modal('show');
            }
        });

        // Receive the sendUserCount event from the AppHub
        AppHub.on('sendUserCount', updateCount);

        // Here we'll invoke the 'getUserCount' event and hook into the promise.
        // When the promise resolves (thanks to then), we call the updateCount method.
        //
        AppHub.invoke('getUserCount').then(updateCount);

        //this.cards = [ 'The invisible hand' , 'waiting until marriage' ];

        //this.players = { 'Tong': 0, 'Erin': 0 };

        //this.question = "Instead of coal, Santa now gives the bad children ?"

    }

    // Register the 'AppController' to the 'app' module.
    angular.module('app').controller('AppController', AppController);

})(window.angular);