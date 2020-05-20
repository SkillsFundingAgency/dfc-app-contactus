class DfcAppContactUs {
    constructor() {
        this.compUiPathForContatUs = 'contact-us';
        this.compUiPathForWebchat = 'webchat';
        this.pathForWebchat = '/' + this.compUiPathForWebchat + '/' + 'chat';
        this.pathForlocalWebchat = '/pages/chat';
    }

    initialise() {
        if (window.location.pathname.endsWith(this.pathForWebchat) || window.location.pathname.endsWith(this.pathForlocalWebchat)) {
            this.initialiseWebchatView();
        }
    }

    initialiseWebchatView() {
        //Have to inject the iFrame as we cant have html in the <script> block
        //<noscript> iFrame in the markup is use when no JS
        var hideProductionWarning = document.getElementById('HideProductionWarning');
        if (hideProductionWarning) {
            if (hideProductionWarning.value.toLowerCase() === 'true') {
                var webchatProductionWarning = document.getElementById('WebchatProductionWarning');
                if (webchatProductionWarning) {
                    webchatProductionWarning.classList = 'DfcAppContactUs-Hide';
                }
            }
        }

        var chatUrl = document.getElementById('ChatUrl');
        if (chatUrl) {
            var iFrameContainer = document.getElementById('chatcontainer');
            if (iFrameContainer) {
                var iFrame = document.createElement('iframe');
                iFrame.setAttribute('id', 'webchatframe');
                iFrame.src = chatUrl.value + '?GAID=' + CompUiUtilties.getCookie('_ga');
                iFrame.classList = 'DfcAppContactUs-Webchat';
                iFrame.scrolling = 'no';
                iFrame.title = 'webchat';

                iFrameContainer.appendChild(iFrame);
                iFrameContainer.style.cssText = 'width:100%; height: 1200px; -webkit-overflow-scrolling:touch';
            }
        }
    }
}
