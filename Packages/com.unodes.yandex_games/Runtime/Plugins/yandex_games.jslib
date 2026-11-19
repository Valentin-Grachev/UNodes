
function ConvertJsStringToUnityString(value) {
    var returnStr = value ? String(value) : "";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  }

mergeInto(LibraryManager.library, {
  
  _ApiIsPaused: function() {
    return isPaused ? 1 : 0;
  },

  _CheckSdkInit: function() {
    return (typeof unity !== 'undefined' && unity && ysdk && player) ? 1 : 0;
  },

  _GetLanguageCode: function() {
    return ConvertJsStringToUnityString(ysdk.environment.i18n.lang);
  },

  _GetServerTimeMillis: function() {
    return ysdk.serverTime();
  },

  _GetDeviceType: function() {
    return ConvertJsStringToUnityString(ysdk.deviceInfo.type);
  },

  _GameReady: function() {
    ysdk.features.LoadingAPI.ready();
  },


  _ShowInterstitial: function() {
    ysdk.adv.showFullscreenAdv({
      callbacks: {

        onOpen: () => {
          unity.SendMessage('YandexGamesSDK', 'OnInterstitialOpened');
        },

        onClose: function(wasShown) {
          if (wasShown) {
            unity.SendMessage('YandexGamesSDK', 'OnInterstitialClosed', '1');
          } else {
            unity.SendMessage('YandexGamesSDK', 'OnInterstitialClosed', '0');
          }
        },

        onError: (e) => {
          console.log('Interstitial ad error:', e);
        }
      }
    });
  },

  _ShowRewarded: function() {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          unity.SendMessage('YandexGamesSDK', 'OnRewardedOpened'); 
        },

        onRewarded: () => {
          unity.SendMessage('YandexGamesSDK', 'OnRewardedRewarded');
        },

        onClose: (wasShown) => {
          if (wasShown) {
            unity.SendMessage('YandexGamesSDK', 'OnRewardedClosed', '1');
          } else {
            unity.SendMessage('YandexGamesSDK', 'OnRewardedClosed', '0');
          }
        }, 

        onError: (e) => {
          console.log('Rewarded ad error:', e);
        }
      }
    });
  },

  _ShowBanner: function() {
    ysdk.adv.showBannerAdv();
  },

  _HideBanner: function() {
    ysdk.adv.hideBannerAdv();
  },

  _RequestSaves: function(saveKey) {
    const saveKeyString = UTF8ToString(saveKey);

    player.getData([saveKeyString]).then((data) => {
      if (data[saveKeyString]) {
        unity.SendMessage('YandexGamesSDK', 'OnSaveDataReceived', data[saveKeyString]);
      } else {
        unity.SendMessage('YandexGamesSDK', 'OnSaveDataReceived', '');
      }
    });
  },

  _SetSaves: function(saveKey, data) {
    const saveKeyString = UTF8ToString(saveKey);
    const dataString = UTF8ToString(data);

    player.setData({ [saveKeyString]: dataString }, true).then(() => {
      unity.SendMessage('YandexGamesSDK', 'OnSetSavesSuccess');
    });
  },


  _RequestCanReview: function() {
    ysdk.feedback.canReview().then(function(result) {
      unity.SendMessage('YandexGamesSDK', 'OnCanReviewHandled', result.value ? '1' : '0');
    })
    .catch(function(e) {
      console.log('canReview error:', e);
      unity.SendMessage('YandexGamesSDK', 'OnCanReviewHandled', '0');
    });
  },


  _RequestReview: function() {
    ysdk.feedback.canReview().then(function(result) {

      if (!result.value) {
        unity.SendMessage('YandexGamesSDK', 'OnReviewHandled', '0');
        return;
      }

      return ysdk.feedback.requestReview().then(function(reviewResult) {
          unity.SendMessage('YandexGamesSDK', 'OnReviewHandled', reviewResult.feedbackSent ? '1' : '0');
        });
      })
      .catch(function(e) {
            console.log('requestReview error:', e);
            unity.SendMessage('YandexGamesSDK', 'OnReviewHandled', '0');
      });

    },

  _GameplayStart: function() {
    if (ysdk.features.GameplayAPI) {
      ysdk.features.GameplayAPI.start();
    }
  },

  _GameplayStop: function() {
    if (ysdk.features.GameplayAPI) {
      ysdk.features.GameplayAPI.stop();
    }
  },




});