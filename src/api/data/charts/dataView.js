//import 'chartjs-adapter-date-fns'
//const Chart = require('chart.js'); if running in Node.js
//Fetch data


const url = "http://192.168.15.3:5000/monitor/getLevels?n=15";

fetch(url)
    .then(response => response.json())
    .then(data => {
        //Get the waterlevel and time data from the json
        const WaterLevels = data.map(item => item.waterLevel);  
        const RecordTimes = data.map(item => item.timeOfRecord);
        console.log(WaterLevels);
        console.log(RecordTimes);
        console.log(data);


        //Create chart
        //Get chart canvas element in page.html
        const ctx = document.getElementById('dataChart').getContext('2d'); 

        //Creates a chart whilst defining type, desired values, color... etc
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: RecordTimes,
                datasets: [{
                    label: 'Water Level',
                    data: WaterLevels,
                    fill: false,
                    borderColor: 'rgb(75, 192, 192)',
                    tension: 0
                }]
            },
            options: {
                scales: {
                    x: {
                        type: 'time',
                        time: {
                            unit: 'minute',
                        },
                        title:{
                            display: true,
                            text: 'Time'
                        }
                    },
                    y:{
                        display: true,
                        title:{
                            display: true,
                            text: 'Water Level'
                        },
                        ticks:{
                            min: 0.0,
                            max: 10.0
                        }
                    }
                },
                responsive: false,
                width: 800,
                height: 400
            }
        })
    })