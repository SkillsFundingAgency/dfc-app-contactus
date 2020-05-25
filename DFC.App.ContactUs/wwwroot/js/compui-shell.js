class CompUiShell {
    constructor(allChildApps) {
        this.allChildApps = allChildApps;

        this.validation = new CompUiValidation();
    }

    initialise() {
        this.validation.initialise();
        this.allChildApps.forEach(f => f.initialise());
    }
}

window.onload = void new CompUiShell(
    [
        new DfcAppContactUs()
    ]).initialise();
