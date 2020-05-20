class CompUiChildApps {
    constructor(allChildApps) {
        this.allChildApps = allChildApps;
    }

    initialise() {
        this.allChildApps.forEach(f => f.initialise());
    }
}

window.onload = function () {
    var compUiChildApps = new CompUiChildApps(
        [
            new DfcAppContactUs()
        ]
    );

    compUiChildApps.initialise();
}();
