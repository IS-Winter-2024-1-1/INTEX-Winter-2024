import azure.functions as func
import logging

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)

@app.route(route="isitfraud")
def isitfraud(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    
    try:
        req_body = req.get_json()
        
    except:
        return func.httpResponse(
            "Internal server error!",
            status_code=500
        )
    #########################
    import pandas as pd

    def load_pickle(file_name):
        import pickle
        model = pickle.load(open(file_name, "rb"))
        return model

    def preprocess_data(data_dict):
        # Extract order data and customer data from the big dictionary
        order_data = data_dict['order_data']
        customer_data = data_dict['customer_data']

        # Convert order dictionary to DataFrame
        order_df = pd.DataFrame([order_data])

        # Convert customer dictionary to DataFrame
        customer_df = pd.DataFrame([customer_data])

        # Merge order and customer DataFrames on customer_ID
        merged_df = pd.merge(order_df, customer_df, on='customer_ID')

        # Create binary indicators for country_of_transaction and shipping_address
        merged_df['country_of_transaction_United Kingdom'] = (merged_df['country_of_transaction'] == 'United Kingdom').astype(int)
        merged_df['shipping_address_United Kingdom'] = (merged_df['shipping_address'] == 'United Kingdom').astype(int)
        merged_df['country_of_residence_United Kingdom'] = (merged_df['country_of_residence'] == 'United Kingdom').astype(int)

        # Drop unnecessary columns
        merged_df.drop(columns=['transaction_ID','customer_ID', 'date','birth_date', 'entry_mode', 'type_of_transaction', 'bank', 'type_of_card', 'country_of_transaction', 'shipping_address', 'country_of_residence', 'first_name', 'last_name', 'gender', 'age', 'day_of_week'], inplace=True)

        return merged_df

    # Test the function with example data
    # data_dict = {
    #     "order_data": {
    #         "transaction_ID": 100006,
    #         "customer_ID": 1,
    #         "date": '11/9/2023',
    #         "day_of_week": 'Wed',
    #         "time": 23,
    #         "entry_mode": 'CVC',
    #         "amount": 20,
    #         "type_of_transaction": 'Online',
    #         "country_of_transaction": 'USA',
    #         "shipping_address": 'India',
    #         "bank": 'Barclays',
    #         "type_of_card": 'Visa'
    #     },
    #     "customer_data": {
    #         "customer_ID": 1,
    #         "first_name": "William",
    #         "last_name": "Turner",
    #         "birth_date": '1995-07-03',
    #         "country_of_residence": "United Kingdom",
    #         "gender": "M",
    #         "age": 25.2
    #     }
    # }

    preprocessed_data = preprocess_data(req_body)
    model = load_pickle('lego_fraud2.sav')
    prediction = model.predict(preprocessed_data)[0]

   
    #########################

    return func.HttpResponse(
             str({"fraud": prediction}),
             status_code=200
        )