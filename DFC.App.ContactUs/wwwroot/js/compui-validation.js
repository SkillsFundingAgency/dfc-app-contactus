class CompUiValidation {
    initialise() {
        this.formValidationSelectorClassName = 'form.compui-validation';
        this.fieldErrorClassName = 'field-validation-error';
        this.fieldValidClassName = 'field-validation-valid';
        this.govukErrorMessageClassName = "govuk-error-message";
        this.govukGroupErrorClassName = 'govuk-form-group--error';
        this.govukGroupClassName = 'govuk-form-group';
        this.govukInputClassName = 'govuk-input';
        this.govukInputErrorClassName = 'govuk-input--error';
        this.govukTextAreaErrorClassName = 'govuk-textarea--error';
        this.compUiValidationForDate = 'CompUiValidationForDate';

        if ($(this.formValidationSelectorClassName).length > 0) {
            this.InitialiseFieldValidationClassSwaps();
            this.InitialiseDateFieldValidation();
        }
    }

    InitialiseFieldValidationClassSwaps() {
        // override add/remove class to trigger a change event
        (function (func) {
            $.fn.addClass = function () {
                func.apply(this, arguments);
                this.trigger('classChanged');
                return this;
            }
        })($.fn.addClass); // pass the original function as an argument

        (function (func) {
            $.fn.removeClass = function () {
                func.apply(this, arguments);
                this.trigger('classChanged');
                return this;
            }
        })($.fn.removeClass);

        // trigger the add/remove class changes to add validation error class to the parent form group
        var formValidationSelectors = this.formValidationSelectorClassName + ' .' + this.fieldErrorClassName + ', ' + this.formValidationSelectorClassName + ' .' + this.fieldValidClassName;
        var thisClass = this;

        $(formValidationSelectors).each(function () {
            $(this).on('classChanged', function () {
                thisClass.ReplaceFieldErrorClasses(this);
            });
        });

        $(formValidationSelectors).each(function () {
            thisClass.ReplaceFieldErrorClasses(this);
        });
    }

    InitialiseDateFieldValidation() {
        var thisClass = this;

        $.validator.unobtrusive.adapters.add(
            thisClass.compUiValidationForDate, [ 'properties' ], function (options) {
                options.rules[thisClass.compUiValidationForDate] = options.params;
                options.messages[thisClass.compUiValidationForDate] = options.message;
            }
        );

        $.validator.addMethod(thisClass.compUiValidationForDate, function (value, element, params) {
            var displayName = params;
            var displayNameLowerCase = displayName.toLowerCase();
            var dateFormGroup = $(element).closest('.' + thisClass.govukGroupClassName)[0];
            var validMsg = $(dateFormGroup).find('.' + thisClass.govukErrorMessageClassName)[0];
            var validMsgId = validMsg.id;
            var inputFields = $(dateFormGroup).find('.' + thisClass.govukInputClassName);
            var isForDateOnly = inputFields.length == 3;
            var dayString = $(inputFields[0]).val();
            var monthString = $(inputFields[1]).val();
            var yearString = $(inputFields[2]).val();
            var hourString = isForDateOnly ? '0' : $(inputFields[3]).val();
            var minuteString = isForDateOnly ? '0' : $(inputFields[4]).val();

            for (var i = 0; i < inputFields.length; i++) {
                thisClass.ValidationMessageShow(inputFields[i], validMsgId, '');
            }

            if (isForDateOnly && dayString === '' && monthString === '' && yearString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[0], validMsgId, 'Enter a date for ' + displayNameLowerCase);
                return false;
            }
            if (!isForDateOnly && dayString === '' && monthString === '' && yearString === '' && hourString === '' && minuteString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[0], validMsgId, 'Enter a date and time for ' + displayNameLowerCase);
                return false;
            }

            if (dayString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[0], validMsgId, 'Enter a day for ' + displayNameLowerCase);
                return false;
            }
            if (monthString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[1], validMsgId, 'Enter a month for ' + displayNameLowerCase);
                return false;
            }
            if (yearString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[2], validMsgId, 'Enter a year for ' + displayNameLowerCase);
                return false;
            }
            if (hourString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[3], validMsgId, 'Enter an hour for ' + displayNameLowerCase);
                return false;
            }
            if (minuteString === '') {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[4], validMsgId, 'Enter a minute for ' + displayNameLowerCase);
                return false;
            }

            if (!CompUiUtilties.isInt(dayString)) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[0], validMsgId, displayName + ' requires numbers for the day');
                return false;
            }
            if (!CompUiUtilties.isInt(monthString)) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[1], validMsgId, displayName + ' requires numbers for the month');
                return false;
            }
            if (!CompUiUtilties.isInt(yearString)) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[2], validMsgId, displayName + ' requires numbers for the year');
                return false;
            }
            if (!CompUiUtilties.isInt(hourString)) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[3], validMsgId, displayName + ' requires numbers for the hour');
                return false;
            }
            if (!CompUiUtilties.isInt(minuteString)) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[4], validMsgId, displayName + ' requires numbers for the minute');
                return false;
            }

            var dayValue = parseInt(dayString);
            var monthValue = parseInt(monthString);
            var yearValue = parseInt(yearString);
            var hourValue = parseInt(hourString);
            var minuteValue = parseInt(minuteString);
            var today = new Date();

            if (monthValue < 1 || monthValue > 12) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[1], validMsgId, displayName + ' requires a valid month');
                return false;
            }
            if (yearValue < 1900 || yearValue > today.getFullYear() + 10) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[2], validMsgId, displayName + ' requires a valid year');
                return false;
            }

            var daysInMonth = CompUiUtilties.getDaysInMonth(monthValue, yearValue);
            if (dayValue < 1 || dayValue > daysInMonth) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[0], validMsgId, displayName + ' requires a valid day for this month');
                return false;
            }

            if (hourValue < 0 || hourValue > 23) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[3], validMsgId, displayName + ' requires a valid hour');
                return false;
            }
            if (minuteValue < 0 || minuteValue > 59) {
                $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[4], validMsgId, displayName + ' requires a valid minute');
                return false;
            }

            var dateObject = new Date(yearValue, monthValue - 1, dayValue, hourValue, minuteValue);

            if (CompUiUtilties.isValidDate(dateObject)) {
                return true;
            }

            $.validator.messages.dateValidation = thisClass.ValidationMessageShow(inputFields[0], validMsgId, displayName + ' is an invalid date');

            return false;
        }, '');
    }

    ValidationMessageShow(inputElement, validMsgId, message) {
        var validMsg = $('#' + validMsgId);

        validMsg.html(message);

        if (message && message != "") {
            validMsg.removeClass(this.fieldValidClassName);
            validMsg.addClass(this.fieldErrorClassName);
            $(inputElement).addClass(this.govukInputErrorClassName);
        } else {
            validMsg.removeClass(this.fieldErrorClassName);
            validMsg.addClass(this.fieldValidClassName);
            $(inputElement).removeClass(this.govukInputErrorClassName);
        }

        return message;
    }

    ReplaceFieldErrorClasses(validMsg) {
        var formGroup = $(validMsg).closest('.' + this.govukGroupClassName);
        var inputElementId = $(validMsg).data('valmsg-for').replace('.', '_');
        var errorCount = formGroup.find('.' + this.fieldErrorClassName).length;

        if (errorCount > 0) {
            formGroup.addClass(this.govukGroupErrorClassName);
        } else {
            formGroup.removeClass(this.govukGroupErrorClassName);
        }

        var inputElement = formGroup.find('#' + inputElementId);
        var className = inputElement.prop("tagName") === 'TEXTAREA' ? this.govukTextAreaErrorClassName : this.govukInputErrorClassName;

        if (validMsg.classList.contains(this.fieldErrorClassName)) {
            inputElement.addClass(className);
        } else {
            inputElement.removeClass(className);
        }
    }
}