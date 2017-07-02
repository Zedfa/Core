var jqueryDateTimeModule = new SepehrModule.MainModule("jqueryDateTimeModule", ['dateInfoServiceModule']);
jqueryDateTimeModule.addDirective("jqueryDate", ["$compile", "dateInfoService", function ($compile, dateInfoService) {

    return {
        restrict: "E",
        replace: true,
        scope: {
            inputId: "@",
            displayMember: "=",
            watermark: "@",
            yearRange: "@",
            dateFormat: "@",
            buttonShow: "=",
            width: "@",
            onSelectedDate: "&",
            selectedDate: "=",
            setCurrentDate: "=",
            enableCurrentMonth:"=",
            validators: "="
        },
        controller: ["$scope", "$element", function ($scope, $element) {

            if (!$scope.selectedDate ) {
                if ($scope.setCurrentDate) {

                    dateInfoService.GetTodaysShamsiDate().then((date) => {

                        $scope.selectedDate = date.replace(/\//g, '-');
                    });
                }
            }


        }],
        link: function (scope, elem, attrs) {

            scope.$watch('selectedDate', function (value) {
                if (value) {
                    scope.displayMember = value.replace(/\//g, '-');
                }
                else {
                    scope.displayMember = value;
                }

            });


            scope.$watch('displayMember', function (value) {
                if (value) {
                    scope.selectedDate = value.replace(/-/g, '/');
                }
                else {
                    scope.selectedDate = value;
                }
            });
            var dateElement = elem.find("input[type=text]");

            var showButton = !scope.buttonShow || scope.buttonShow === true ? 'button' : 'focus',
                dFormat = !scope.dateFormat ? 'yy/mm/dd' : scope.dateFormat,
                yRange = !scope.yearRange ? 'c-90:c+10' : scope.yearRange,
                maxDate = scope.enableCurrentMonth ? "0" : "";

            dateElement.datepicker({
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                yearRange: yRange,
                dateFormat: dFormat,
                maxDate: maxDate,
                onSelect: function (dateText, inst) {

                    scope.onSelectedDate({ args: { dateText: dateText, instance: inst } });

                    scope.$apply(function () {
                        var displayMember = dateText;
                        scope.selectedDate = displayMember.replace(/-/g, '/')
                    });
                },
                beforeShow: function (element, inst) {
                    inst.id = element.id;
                },
                onClose: function (dateText, inst) {
                },
                showOn: showButton
            });

            $('#ui-datepicker-div').css('zIndex', '10000003');
            $('#ui-datepicker-div').css('font-size', 'x-small');

            elem.width(scope.width ? scope.width : "auto");
          
        },
        template: '<span> <input id="{{inputId}}"  class="k-textbox" type="text" ng-model="displayMember" validations="validators" ></span>'
    };
}]); 
