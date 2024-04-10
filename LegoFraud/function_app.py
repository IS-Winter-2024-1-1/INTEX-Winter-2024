import azure.functions as func
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

@app.route(route="isitfraud")
def isitfraud(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    
    try:
        req_body = req.get_json()
        
    except:
        return func.httpRespone(
            "Internal server error!",
            status_code=500
        )
    #########################
    import pickle 
    import pandas as pd

    def date_function(df, column_to_date):
        #import the proper packages
        #from datetime import datetime
        #import calendar
        import pandas as pd

        #cast the column to the data type of a date
        df[column_to_date] = pd.to_datetime(df[column_to_date])

        #make 3 new columns(day of the month, month of the year, and year)

        df['day_of_month'] = df[column_to_date].dt.day

        df['month_of_year'] = df[column_to_date].dt.month

        df['year'] = df[column_to_date].dt.year
        
        df['day_of_month'] = df['day_of_month'].astype(str)
        df['month_of_year'] = df['month_of_year'].astype(str)
        df['year'] = df['year'].astype(str)

        #drop the original date column
        df.drop(columns = column_to_date, inplace = True)

        return df
    
    def load_pickle(file_name):
        import pickle
        model = pickle.load(open(file_name, "rb"))
        return model


    # test_dictionary = {
    #         "transaction_ID": 100006,
    #         "customer_ID": 1,
    #         "date": '11/9/2023',
    #         "day_of_week": 'Wed',
    #         "time": 24,
    #         "entry_mode": 'CVC',
    #         "amount": 20,
    #         "type_of_transaction": 'Online',
    #         "country_of_transaction": 'USA',
    #         "shipping_address": 'India',
    #         "bank": 'Barclays',
    #         "type_of_card": 'Visa'
    #     }

    df_test = pd.DataFrame([req_body])

    columns_to_drop = ['transaction_ID', 'customer_ID']

    df_test.drop(columns = columns_to_drop, inplace = True)

    df_test = date_function(df_test, 'date')

    more_columns_to_drop = ['day_of_week', 'entry_mode', 'type_of_transaction', 'bank', 'type_of_card','day_of_month',
                            'month_of_year', 'year']

    df_test.drop(columns = more_columns_to_drop, inplace = True)

    columns_mapping = {
        'country_of_transaction': 'country_of_transaction_United Kingdom',
        'shipping_address': 'shipping_address_United Kingdom'
    }

    df_test = df_test.rename(columns=columns_mapping)

    df_test['country_of_transaction_United Kingdom'] = df_test['country_of_transaction_United Kingdom'] == 'United Kingdom'

    df_test['shipping_address_United Kingdom'] = df_test['shipping_address_United Kingdom'] == 'United Kingdom'

        
    saved_model_path = "lego_fraud.sav"
    loaded_model = load_pickle(saved_model_path)

    prediction = loaded_model.predict(df_test)[0]

    


    #########################

    return func.HttpResponse(
             str({"fraud": prediction}),
             status_code=200
        )