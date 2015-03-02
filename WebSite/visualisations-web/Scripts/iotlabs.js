$(function () {
	// Get the device ID from a hash parameter, i.e. http://something.azurewebsites.net/#deviceid
	var deviceId = document.URL.split('#')[1];
	if (!deviceId) deviceId = 'tom-snappy'; // default device

    var loadAvgProxy = $.connection.loadAvgHub;
    loadAvgProxy.client.pump = function(readings) {
        drawLoadAvgChart(readings);
    };

    $.connection.hub.start()
        .done(function () {
            console.log('connected, id=' + $.connection.hub.id);
        	loadAvgProxy.server.startReadingPump(deviceId)
                .done(function() { console.log('started pump for loadavg'); })
                .fail(function(e) { console.error(e) });
        })
        .fail(function() { console.log('Could not Connect!'); });

    var drawLoadAvgChart = function (readings) {
        var categories = new Array(readings.length);
        var series = new Array(1);
        series[0] = { name: 'Load1', data: new Array(readings.length) };
        for (var i = 0; i < readings.length; i++) {
            categories[i] = readings[i].Time;
            series[0].data[i] = readings[i].Load1;
        }
        //console.log(series[0].data);

        $('#loadavgMonitorChart').highcharts({
            title: {
                text: 'Last Load Average Readings',
                x: -20 //center
            },
            subtitle: {
            	text: deviceId,
                x: -20
            },
            xAxis: {
                categories: categories,
                labels: {
                    enabled: false
                },
                reversed: true
            },
            yAxis: {
                title: {
                    text: 'Load'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: ''
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            plotOptions: {
                series: {
                    animation: false
                }
            },
            series: series
        });
    }
});