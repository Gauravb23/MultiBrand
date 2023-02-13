$(function() {
 var doc = document, body = $(doc.body), win = window, ww= $(window).width(),
 allTransactionsChart = {
 	initTransactionsChart: function() {
 		var totalData_bhamaEmitra =
 			[{
                name: 'Emitra',
                y: 109653756
            }, {
                name: 'Bhamashah',
                y: 89234574
            },
            {
                name: 'M&H',
                y: 28569674
            }, {
                name: 'ePDS',
                y: 42753185
            },
            {
                name: 'RPSC',
                y: 6125568
            },
            {
                name: 'Excise',
                y: 7409080
            }];
         var yearData_bhamaEmitra = 
            [{
                name: 'Emitra',
                y: 45589319
            }, {
                name: 'Bhamashah',
                y: 68229557
            },
            {
			    name: 'M&H',
			    y: 11048332
			}, 
			{
			    name: 'ePDS',
			    y: 41721212
			},
			{
			    name: 'RPSC',
			    y: 525975
			}, 
			 {
			    name: 'Excise',
			    y: 866969
			}, 
            ];
         var todayData_bhamaEmitra = 
            [{
                name: 'Emitra',
                y: 35176
            }, {
                name: 'Bhamashah',
                y: 135517
            },
            {
			    name: 'M&H',
			    y: 31851
			}, {
			    name: 'ePDS',
			    y: 13114
			}
			, {
			    name: 'RPSC',
			    y: 3181
			}, {
			    name: 'Excise',
			    y: 283
			}
            ];
      var totalData = 
             [{
                name: 'Sampark',
                y: 884708
            }, {
                name: 'Labour',
                y: 286320
            }, {
                name: 'SJE',
                y: 171679
            },
            {
                name: 'RTI',
                y: 12251
            }, {
                name: 'Raj Mandi',
                y: 1365055
            }, {
                name: 'RAJFAB',
                y: 2520
            },{
                name: 'Industries',
                y: 17288
            },
            {
                name: 'Transport',
                y: 32882
            }, {
                name: 'RSPCB',
                y:38277
            }, {
                name: 'Housing Board',
                y: 2056432
            }, {
                name: 'Police',
                y: 73772
            },
             {
                name: 'Election',
                y: 279172
            },{
                name: 'Lites',
                y: 287802
            }];

            var yearData = 
            [{
                name: 'Sampark',
                y: 228435
            }, {
                name: 'Labour',
                y: 228001
            }, {
                name: 'SJE',
                y: 98409
            },
            {
                name: 'RTI',
                y: 5809
            }, {
                name: 'Raj Mandi',
                y: 713261
            }, {
                name: 'RAJFAB',
                y: 2520
            }, {
                name: 'Industries',
                y: 17288
            },
            {
                name: 'Transport',
                y: 23798
            }, {
                name: 'RSPCB',
                y:38277
            }, {
                name: 'Housing Board',
                y: 68690
            }, {
                name: 'Police',
                y: 24818
            },
          	{
                name: 'Election',
                y: 279172
            },{
                name: 'Lites',
                y: 41086
            }];
            var todayData = 
            [{
                name: 'Sampark',
                y: 315
            }, {
                name: 'Labour',
                y: 358
            }, {
                name: 'SJE',
                y: 74
            },
            {
                name: 'RTI',
                y: 12
            }, {
                name: 'Raj Mandi',
                y: 366
            }, {
                name: 'RAJFAB',
                y: 0
            }, {
                name: 'Industries',
                y: 18
            },
            {
                name: 'Transport',
                y: 57
            }, {
                name: 'RSPCB',
                y:11
            }, {
                name: 'Housing Board',
                y: 26
            }, {
                name: 'Police',
                y: 31
            }, {
                name: 'Election',
                y: 619
            }, {
                name: 'Lites',
                y: 22
            }];
            var chartData;
            var currentFilter = $('.filter button.active').data('filter');

            if(currentFilter == 'all'){
              chartData = totalData;
              bhamaEmitra_data = totalData_bhamaEmitra
            }
            else if(currentFilter == 'year'){
              chartData = yearData;  
              bhamaEmitra_data = yearData_bhamaEmitra
            }
            else if(currentFilter == 'month'){
              chartData = yearData;  
              bhamaEmitra_data = yearData_bhamaEmitra
            }
            else if(currentFilter == 'today'){
              chartData = todayData;  
              bhamaEmitra_data = todayData_bhamaEmitra
            }
            

    // Create the chart
    $('#container').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Other transactions'
        },
        credits: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Total percent'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                minPointLength: 10
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{point.y:.1f}</td>' +
            '</tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },

        series: [{
            colorByPoint: true, 
            data: chartData
        }]
       
    });

    // Create the all transactions chart
    $('#emitra-bhamashah-chart').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: 'Major transactions'
        },
        credits: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Total percent'
            }
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                borderWidth: 0,
                minPointLength: 10
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
            pointFormat: '<tr><td style="color:{series.color};padding:0">{point.y:.1f}</td>' +
            '</tr>',
            footerFormat: '</table>',
            shared: true,
            useHTML: true
        },

        series: [{
            colorByPoint: true, 
            data: bhamaEmitra_data
        }]
       
    });

    }, //initTransactionsChart
    initChartOnClick: function(){
      $('.showChart').on('click', function() {
        allTransactionsChart.initTransactionsChart();
      });
    }, //initChartOnClick
    filterChartBtns: function(){
        $('.allTrasactions .filter').find('button').on('click', function(){
          $(this).siblings('.active').removeClass('active');
          $(this).addClass('active');
          allTransactionsChart.initTransactionsChart();
        });
    } //filterChartBtns
 }
 
 allTransactionsChart.initChartOnClick();
 allTransactionsChart.filterChartBtns();
});